using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http;

namespace Utilities.Shared.ApiClient
{
    public class ApiErrorReturnExtension : IHttpActionResult
    {

        #region Private Vars

        HttpStatusCode _code;
        string _message;
        HttpRequestMessage _request;

        #endregion Private Vars


        public ApiErrorReturnExtension(HttpStatusCode code, string message, HttpRequestMessage request)
        {
            _message = message;
            _request = request;
            _code = code;
        }

        public Task<HttpResponseMessage> ExecuteAsync(CancellationToken cancellationToken)
        {
            return Task.FromResult(Execute());
        }

        public HttpResponseMessage Execute()
        {
            HttpResponseMessage resp = new HttpResponseMessage(_code);
            resp.Content = new StringContent(_message);
            resp.RequestMessage = _request;
            return resp;
        }
    }
}
