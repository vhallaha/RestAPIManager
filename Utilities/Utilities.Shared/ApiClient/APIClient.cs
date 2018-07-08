using Newtonsoft.Json;
using System;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Utilities.Shared.ApiClient
{
    public class APIClient
    {

        #region Private Vars

        private string _jwtBearer = String.Empty;
        private string _apiKey = String.Empty;
        private string _apiSecret = String.Empty;

        private DateTime _tokenExpTime = new DateTime();
        private HttpClient _client = null;
        private Uri _baseUrl = null;


        #endregion Private Vars

        #region Ctor

        /// <summary>
        /// Default Constructor
        /// </summary>
        /// <param name="baseUrl">Domain Url</param>
        /// <param name="apiKey">API Key</param>
        /// <param name="apiSecret">API Secret</param>
        public APIClient(Uri baseUrl, string apiKey, string apiSecret)
        {
            _baseUrl = baseUrl;
            _apiKey = apiKey;
            _apiSecret = apiSecret;
        }

        #endregion Ctor

        #region Methods

        public T Get<T>(string resKey, string endPoint, out ErrorMsgVm errorMsg)
        {
            errorMsg = null;
            var res = Request(resKey, endPoint, HttpMethod.Get);

            if (res.Code != HttpStatusCode.OK)
            {
                if (IsJson(res.Result))
                {
                    errorMsg = JsonConvert.DeserializeObject<ErrorMsgVm>(res.Result);
                    if (!String.IsNullOrWhiteSpace(errorMsg.Message))
                        return default(T);
                }
                else
                {
                    errorMsg = new ErrorMsgVm() { Message = res.Code.ToString() };
                    return default(T);
                }
            }

            return JsonConvert.DeserializeObject<T>(res.Result);
        }

        public T Put<T>(string resKey, string endPoint, object param, out ErrorMsgVm errorMsg)
        {
            errorMsg = null;
            var res = Request(resKey, endPoint, HttpMethod.Put, param);

            if (res.Code != HttpStatusCode.OK)
            {
                if (IsJson(res.Result))
                {
                    errorMsg = JsonConvert.DeserializeObject<ErrorMsgVm>(res.Result);
                    if (!String.IsNullOrWhiteSpace(errorMsg.Message))
                        return default(T);
                }
                else
                {
                    errorMsg = new ErrorMsgVm() { Message = res.Code.ToString() };
                    return default(T);
                }
            }

            return JsonConvert.DeserializeObject<T>(res.Result);
        }

        public T Post<T>(string resKey, string endPoint, object param, out ErrorMsgVm errorMsg)
        {
            errorMsg = null;
            var res = Request(resKey, endPoint, HttpMethod.Post, param);

            if (res.Code != HttpStatusCode.OK)
            {
                if (IsJson(res.Result))
                {
                    errorMsg = JsonConvert.DeserializeObject<ErrorMsgVm>(res.Result);
                    if (!String.IsNullOrWhiteSpace(errorMsg.Message))
                        return default(T);
                }
                else
                {
                    errorMsg = new ErrorMsgVm() { Message = res.Code.ToString() }; 
                    return default(T);
                }
            }

            return JsonConvert.DeserializeObject<T>(res.Result);
        }

        public T Delete<T>(string resKey, string endPoint, out ErrorMsgVm errorMsg)
        {
            errorMsg = null;
            var res = Request(resKey, endPoint, HttpMethod.Delete);

            if (res.Code != HttpStatusCode.OK)
            {
                if (IsJson(res.Result))
                {
                    errorMsg = JsonConvert.DeserializeObject<ErrorMsgVm>(res.Result);
                    if (!String.IsNullOrWhiteSpace(errorMsg.Message))
                        return default(T);
                }
                else
                {
                    errorMsg = new ErrorMsgVm() { Message = res.Code.ToString() };
                    return default(T);
                }
            }

            return JsonConvert.DeserializeObject<T>(res.Result);
        }

        #endregion Methods

        #region Helper

        private (string Result, HttpStatusCode Code) Request(string resKey, string endPoint, HttpMethod method, object param = null)
        {
            var client = GetClient();
            if (client == null)
            {
                var message = JsonConvert.SerializeObject(new ErrorMsgVm()
                {
                    Message = "Failed to create client"
                });

                return (message, HttpStatusCode.InternalServerError);
            }

            client.DefaultRequestHeaders.Add("key", resKey);

            // Create Request.
            Uri requestUri = new Uri(_baseUrl, endPoint);
            var request = new HttpRequestMessage(method, requestUri);
            StringContent content = null;

            if (param != null) 
                content = new StringContent(JsonConvert.SerializeObject(param), Encoding.UTF8, "application/json");
             
            Task<HttpResponseMessage> result = null; 
            switch (method.Method)
            {
                case "POST":
                    result = client.PostAsync(requestUri, content);
                    break;
                case "PUT":
                    result = client.PutAsync(requestUri, content);
                    break;
                case "DELETE":
                    result = client.DeleteAsync(requestUri);
                    break;
                case "GET":
                    result = client.GetAsync(requestUri);
                    break;                        
            }

            var resJson = result.Result.Content.ReadAsStringAsync();
            return (resJson.Result, result.Result.StatusCode);
        }

        /// <summary>
        /// Returns a fresh HttpClient
        /// </summary>
        /// <returns></returns>
        private HttpClient GetClient()
        {
            var result = GrabAuthorization().Result;
            if (!result)
                return _client;

            _client = new HttpClient();
            _client.BaseAddress = _baseUrl;
            _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + _jwtBearer);
            return _client;

        }

        /// <summary>
        /// Returns true if the http client needs to be updated.
        /// </summary>
        /// <returns>bool</returns>
        private async Task<bool> GrabAuthorization()
        {
            if (_tokenExpTime == new DateTime() ||
               _tokenExpTime >= DateTime.UtcNow)
            {
                using (var client = new HttpClient())
                {
                    try
                    {
                        // TODO : Grab this information from the configuration.
                        client.BaseAddress = _baseUrl;
                        var resp = await client.PostAsJsonAsync("/api/resourcemanager/token/Authorize/", new
                        {
                            APIKey = _apiKey,
                            APISecret = _apiSecret
                        }).ConfigureAwait(false);

                        // Failed the response
                        if (!resp.IsSuccessStatusCode)
                            return false;

                        var context = await resp.Content.ReadAsStringAsync().ConfigureAwait(false);
                        var data = JsonConvert.DeserializeObject<dynamic>(context);

                        _jwtBearer = data.Token;
                        _tokenExpTime = data.ExpireDate;
                    }
                    catch
                    { return false; }
                }

                return true;
            }

            return false;
        }
        
        /// <summary>
        /// Validates if it can deserialize string as json object
        /// </summary>
        /// <param name="input">Input</param>
        /// <returns>bool</returns>
        public static bool IsJson(string input)
        {
            input = input.Trim();
            return input.StartsWith("{") && input.EndsWith("}") || input.StartsWith("[") && input.EndsWith("]");
        }
        #endregion Helper
    }
}
