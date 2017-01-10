using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace TicketSystem.Helper
{
    /// <summary>
    /// Class prevents crossorigin errors.
    /// </summary>
    public class Cors : DelegatingHandler
    {
        private const string Origin = "Origin";
        private const string AccessControlRequestMethod = "Access-Control-Request-Method";
        private const string AccessControlRequestHeaders = "Access-Control-Request-Headers";
        private const string AccessControlAllowOrigin = "Access-Control-Allow-Origin";
        private const string AccessControlAllowMethods = "Access-Control-Allow-Methods";
        private const string AccessControlAllowHeaders = "Access-Control-Allow-Headers";

        protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request,
            CancellationToken cancellationToken)
        {
            return request.Headers.Contains(Origin)
                ? ProcessCorsRequest(request, cancellationToken)
                : base.SendAsync(request, cancellationToken);
        }

        private async Task<HttpResponseMessage> ProcessCorsRequest(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            if (request.Method == HttpMethod.Options)
            {

                var response = new HttpResponseMessage(HttpStatusCode.OK);
                AddCorsResponseHeaders(request, response);
                return response;
            }
            else
            {
                var resp = await base.SendAsync(request, cancellationToken);

                resp.Headers.Remove(AccessControlAllowOrigin);
                resp.Headers.Add(AccessControlAllowOrigin, request.Headers.GetValues(Origin).First());
                return resp;
            }
        }

        private static void AddCorsResponseHeaders(HttpRequestMessage request, HttpResponseMessage response)
        {
            //Delete old headers to avoid problems.
            response.Headers.Remove(AccessControlAllowOrigin);

            //Cors Headers could be checked here instead of just adding the Reqest Header to the response
            response.Headers.Add(AccessControlAllowOrigin, request.Headers.GetValues(Origin).First());

            var accessControlRequestMethod = request.Headers.GetValues(AccessControlRequestMethod).FirstOrDefault();
            if (accessControlRequestMethod != null)
            {
                response.Headers.Add(AccessControlAllowMethods, accessControlRequestMethod);
            }

            var requestedHeaders = string.Join(", ", request.Headers.GetValues(AccessControlRequestHeaders));
            if (!string.IsNullOrEmpty(requestedHeaders))
            {
                response.Headers.Add(AccessControlAllowHeaders, requestedHeaders);
            }
        }
    }
}