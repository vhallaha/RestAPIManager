using Microsoft.Owin.Security.DataHandler.Encoder;
using System;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;

namespace Utilities
{
    /// <summary>
    /// Cryptography
    /// </summary>
    public class Cryptography
    {
        private static byte[] _key = { };
        private static readonly byte[] initVector = { 9, 10, 207, 2, 27, 13, 204, 10 };
        private static readonly string stringKey = "%T=qa4[Yx5dF~'&yW:";

        private static readonly byte[] KEY192 = {
            12, 122, 97, 64, 59, 4, 48, 92,
            15, 224, 88, 43, 16, 255, 37, 77,
            2, 94, 11, 204, 119, 35, 184, 197
        };

        private static readonly byte[] IV192 = {
            30, 17, 65, 77, 12, 7, 2, 33,
            91, 65, 57, 86, 176, 9, 209, 13,
            145, 23, 200, 58, 173, 10, 46, 149
        };

        public static string SystemHashKey
        {
            get { return ConfigurationManager.Get("systemHashKey"); }
        }

        public static string SystemIVKey
        {
            get { return ConfigurationManager.Get("systemIVKey"); }
        }

        public static string SystemSaltKey
        {
            get { return ConfigurationManager.Get("systemSaltKey"); }
        }

        /// <summary>
        /// 24 bit string ecryption
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string Encrypt24Bit(string value)
        {
            TripleDESCryptoServiceProvider des = new TripleDESCryptoServiceProvider();
            MemoryStream memoryStream = new MemoryStream();
            CryptoStream cryptoStream = new CryptoStream(memoryStream, des.CreateEncryptor(KEY192, IV192), CryptoStreamMode.Write);
            StreamWriter streamWriter = new StreamWriter(cryptoStream);

            streamWriter.Write(value);
            streamWriter.Flush();
            cryptoStream.FlushFinalBlock();
            memoryStream.Flush();

            return Convert.ToBase64String(memoryStream.GetBuffer(), 0, Convert.ToInt32(memoryStream.Length));
        }

        /// <summary>
        /// 24 bit string decryption
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string Decrypt24Bit(string value)
        {
            TripleDESCryptoServiceProvider des = new TripleDESCryptoServiceProvider();
            value = value.Replace(" ", "+");
            byte[] buffer = Convert.FromBase64String(value);
            MemoryStream memoryStream = new MemoryStream(buffer);
            CryptoStream cryptoStream = new CryptoStream(memoryStream, des.CreateDecryptor(KEY192, IV192), CryptoStreamMode.Read);
            StreamReader streamReader = new StreamReader(cryptoStream);

            return streamReader.ReadToEnd();
        }

        /// <summary>
        /// 8 bit string ecryption
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string Encrypt8Bit(string value)
        {
            _key = Encoding.UTF8.GetBytes(stringKey.Substring(0, 8));

            DESCryptoServiceProvider des = new DESCryptoServiceProvider();
            Byte[] byteArray = Encoding.UTF8.GetBytes(value);

            MemoryStream memoryStream = new MemoryStream();
            CryptoStream cryptoStream = new CryptoStream(memoryStream,
                des.CreateEncryptor(_key, initVector), CryptoStreamMode.Write);

            cryptoStream.Write(byteArray, 0, byteArray.Length);
            cryptoStream.FlushFinalBlock();

            return Convert.ToBase64String(memoryStream.ToArray());
        }

        /// <summary>
        /// 8 bit string decryption
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string Decrypt8Bit(string value)
        {
            _key = Encoding.UTF8.GetBytes(stringKey.Substring(0, 8));

            TripleDESCryptoServiceProvider des = new TripleDESCryptoServiceProvider();
            value = value.Replace(" ", "+");
            Byte[] byteArray = Convert.FromBase64String(value);

            MemoryStream memoryStream = new MemoryStream();
            CryptoStream cryptoStream = new CryptoStream(memoryStream,
                des.CreateDecryptor(_key, initVector), CryptoStreamMode.Write);

            cryptoStream.Write(byteArray, 0, byteArray.Length);
            cryptoStream.FlushFinalBlock();

            return Encoding.UTF8.GetString(memoryStream.ToArray());
        }

        /// <summary>
        /// Generate a Url safe Base64 string of the hash of the input. Trailing = are stripped from output
        /// </summary>
        /// <param name="rawValue">Value to be hashed</param>
        /// <returns>Url safe Base64 encoding of the hash with = removed</returns>
        public static string GenerateHash(string rawValue)
        {
            return Base64Url.Encode(GenerateHashRaw(rawValue)).TrimEnd('='); //Remove trailing =
        }

        /// <summary>
        /// Generates a hash of the input string (assumes it is UTF8) using the system hash key.
        /// </summary>
        /// <param name="rawValue">Input to hash</param>
        /// <returns>bytes of the hash</returns>
        public static byte[] GenerateHashRaw(string rawValue)
        {
            return GenerateHashRaw(Encoding.UTF8.GetBytes(rawValue));
        }

        /// <summary>
        /// Generates a hash of the input bytes using the system hash key
        /// </summary>
        /// <param name="input">input bytes to hash</param>
        /// <returns>hashed bytes</returns>
        public static byte[] GenerateHashRaw(byte[] input)
        {
            var sysKey = Encoding.UTF8.GetBytes(SystemHashKey);

            using (var hmac = new HMACSHA256(sysKey))
            {
                return hmac.ComputeHash(input);
            }
        }

        public static byte[] GenerateBase64Raw()
        {
            var key = new byte[32];
            RNGCryptoServiceProvider.Create().GetBytes(key);
            return key;
        }

        public static string GenerateBase64UrlFriendly()
        {
            var byteKey = GenerateBase64Raw();
            var base64Key = TextEncodings.Base64Url.Encode(byteKey);

            return base64Key;
        }

        public static string RemoveSpecialCharacters(string str)
        {
            return Regex.Replace(str, "[^a-zA-Z0-9_.]+", "", RegexOptions.Compiled);
        }

        public static string Encrypt(string plainText, string secretKey)
        {
            byte[] plainTextBytes = Encoding.UTF8.GetBytes(plainText);

            byte[] cipherTextBytes;
            byte[] keyBytes;
            using (var deriveBytes = new Rfc2898DeriveBytes(secretKey, Encoding.ASCII.GetBytes(SystemSaltKey)))
            {
                keyBytes = deriveBytes.GetBytes(256 / 8);
            }

            using (var symmetricKey = new RijndaelManaged() { Mode = CipherMode.CBC, Padding = PaddingMode.Zeros })
            using (var encryptor = symmetricKey.CreateEncryptor(keyBytes, Encoding.ASCII.GetBytes(SystemIVKey)))
            using (var memoryStream = new MemoryStream())
            {
                using (var cryptoStream = new CryptoStream(memoryStream, encryptor, CryptoStreamMode.Write))
                {
                    cryptoStream.Write(plainTextBytes, 0, plainTextBytes.Length);
                    cryptoStream.FlushFinalBlock();
                    cipherTextBytes = memoryStream.ToArray();
                }
            }
            return Convert.ToBase64String(cipherTextBytes);
        }

        public static string Decrypt(string encryptedText, string secretKey)
        {
            byte[] cipherTextBytes = Convert.FromBase64String(encryptedText);
            byte[] keyBytes = new Rfc2898DeriveBytes(secretKey, Encoding.ASCII.GetBytes(SystemSaltKey)).GetBytes(256 / 8);
            byte[] plainTextBytes = new byte[cipherTextBytes.Length];
            int decryptedByteCount;

            using (var symmetricKey = new RijndaelManaged() { Mode = CipherMode.CBC, Padding = PaddingMode.None })
            using (var decryptor = symmetricKey.CreateDecryptor(keyBytes, Encoding.ASCII.GetBytes(SystemIVKey)))
            using (var memoryStream = new MemoryStream(cipherTextBytes))
            using (var cryptoStream = new CryptoStream(memoryStream, decryptor, CryptoStreamMode.Read))
            {
                decryptedByteCount = cryptoStream.Read(plainTextBytes, 0, plainTextBytes.Length);
            }
            return Encoding.UTF8.GetString(plainTextBytes, 0, decryptedByteCount).TrimEnd('\0');
        }

        public static string EncryptToUrlFriendly(string value)
        {
            byte[] array = Encoding.ASCII.GetBytes(value);
            return HttpServerUtility.UrlTokenEncode(array);
        }

        public static string DecryptToFromUrlFriendlyToken(string value)
        {
            byte[] array = HttpServerUtility.UrlTokenDecode(value);
            return System.Text.Encoding.Default.GetString(array);
        }
    }

    public class Randomizer
    {
        public static string GenerateAlphaNumeric(int iLength)
        {
            var chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            var random = new Random();
            var result = new string(
                Enumerable.Repeat(chars, iLength)
                          .Select(s => s[random.Next(s.Length)])
                          .ToArray());
            return result;
        }
    }

    public static class Base64Url
    {
        /// <summary>
        /// Encodes bytes into a url safe base64 string, does NOT remove trailing =
        /// </summary>
        /// <param name="input">input bytes to encode</param>
        /// <returns>Url safe Base64 string</returns>
        /// <exception cref="ArgumentNullException">Thrown if input is null</exception>
        public static string Encode(byte[] input)
        {
            var output = Convert.ToBase64String(input);
            output = output.Replace('+', '-'); // 62nd char of encoding
            output = output.Replace('/', '_'); // 63rd char of encoding
            return output;
        }

        /// <summary>
        /// Decode a given url-safe base64 string into its byte array representation. Is tolerant of missing =
        /// </summary>
        /// <param name="input">url safe Base64 string</param>
        /// <returns>Byte array represented by input</returns>
        /// <exception cref="ArgumentNullException">Thrown if input is null</exception>
        /// <exception cref="FormatException">If input is not a valid base64 string</exception>
        public static byte[] Decode(string input)
        {
            if (input == null)
                throw new ArgumentNullException("input");
            var output = input;
            output = output.Replace('-', '+'); // 62nd char of encoding
            output = output.Replace('_', '/'); // 63rd char of encoding
            switch (output.Length % 4) // Pad with trailing '='s
            {
                case 0: break; // No pad chars in this case
                case 1: output += "==="; break; //3 pad chars, shouldn't happen often...
                case 2: output += "=="; break; // Two pad chars
                case 3: output += "="; break; // One pad char
                                              //default shouldn't be hit if we tolerate all 4 possibilities
                default: throw new FormatException("Illegal base64url string, invalid number of characters in input: " + output.Length.ToString());
            }
            var converted = Convert.FromBase64String(output); // Standard base64 decoder
            return converted;
        }
    }
}
