using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Text;

namespace YulsnApiClient.Models
{
    public class YulsnRequestException : HttpRequestException
    {
        public HttpStatusCode StatusCode { get; set; }
        public string ReasonPhrase { get; set; }
        public string ErrorMessage { get; set; }
        public string ErrorBody { get; set; }

        public YulsnRequestException(HttpStatusCode statusCode, string reasonPhrase, string json)
            : base($"Response status code does not indicate success: {(int)statusCode} ({reasonPhrase}).")
        {
            StatusCode = statusCode;
            ReasonPhrase = reasonPhrase;
            ErrorBody = json;
            if (json != null)
            {
                try
                {
                    var error = JsonConvert.DeserializeObject<YulsnError>(json);
                    if (error != null)
                        ErrorMessage = error.Message;
                }
                catch { }
            }
        }

        class YulsnError
        {
            public string Message { get; set; }
        }
    }
}
