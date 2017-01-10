using System.Collections.Generic;
using Newtonsoft.Json;
using RestSharp;

namespace TicketSystem.Helper
{
    public class RequestHelper
    {
        /// <summary>
        /// Performs a POST REST request.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="apiUrl"></param>
        /// <param name="endpoint"></param>
        /// <param name="payload"></param>
        /// <param name="headers"></param>
        /// <returns></returns>
        public static IRestResponse<T> Post<T>(string apiUrl, string endpoint, object payload, Dictionary<string, string> headers = null) where T : new()
        {
            return Request<T>(Method.POST, apiUrl, endpoint, payload, headers);
        }

        /// <summary>
        /// Performs a PUT REST request.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="apiUrl"></param>
        /// <param name="endpoint"></param>
        /// <param name="payload"></param>
        /// <param name="headers"></param>
        /// <returns></returns>
        public static IRestResponse<T> Put<T>(string apiUrl, string endpoint, object payload, Dictionary<string, string> headers = null) where T : new()
        {
            return Request<T>(Method.PUT, apiUrl, endpoint, payload, headers);
        }

        /// <summary>
        /// Performs a PATCH REST request.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="apiUrl"></param>
        /// <param name="endpoint"></param>
        /// <param name="payload"></param>
        /// <param name="headers"></param>
        /// <returns></returns>
        public static IRestResponse<T> Patch<T>(string apiUrl, string endpoint, object payload, Dictionary<string, string> headers = null) where T : new()
        {
            return Request<T>(Method.PATCH, apiUrl, endpoint, payload, headers);
        }

        /// <summary>
        /// Performs a DELETE REST request.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="apiUrl"></param>
        /// <param name="endpoint"></param>
        /// <param name="payload"></param>
        /// <param name="headers"></param>
        /// <returns></returns>
        public static IRestResponse<T> Delete<T>(string apiUrl, string endpoint, object payload, Dictionary<string, string> headers = null) where T : new()
        {
            return Request<T>(Method.DELETE, apiUrl, endpoint, payload, headers);
        }

        /// <summary>
        /// Performs a GET REST request.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="apiUrl"></param>
        /// <param name="endpoint"></param>
        /// <param name="headers"></param>
        /// <returns></returns>
        public static IRestResponse<T> Get<T>(string apiUrl, string endpoint, Dictionary<string, string> headers = null) where T : new()
        {
            return Request<T>(Method.GET, apiUrl, endpoint, null, headers);
        }


        /// <summary>
        /// Performs a REST request.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="method"></param>
        /// <param name="apiUrl"></param>
        /// <param name="endpoint"></param>
        /// <param name="payload"></param>
        /// <param name="headers"></param>
        /// <returns></returns>
        public static IRestResponse<T> Request<T>(Method method, string apiUrl, string endpoint, object payload, Dictionary<string, string> headers = null) where T : new()
        {
            var client = new RestClient(apiUrl);
            var request = new RestRequest(endpoint, method);

            request.AddHeader("content-type", "application/json");
            if (headers == null) return client.Execute<T>(request);

            foreach (var header in headers)
            {
                request.AddHeader(header.Key, header.Value);
            }
            if (payload != null && method != Method.GET)
            {
                request.AddParameter("application/json", JsonConvert.SerializeObject(payload), ParameterType.RequestBody);
            }

            return client.Execute<T>(request);
        }
    }
}