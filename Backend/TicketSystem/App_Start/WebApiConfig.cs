using System.Globalization;
using System.Net.Http.Headers;
using System.Web.Http;
using System.Web.Http.Routing.Constraints;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using TicketSystem.Helper;

namespace TicketSystem
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            // Web API configuration and services
            config.MessageHandlers.Add(new Cors());
            //Always output JSON 
            config.Formatters.JsonFormatter.SupportedMediaTypes.Add(new MediaTypeHeaderValue("text/html"));
            var settings = config.Formatters.JsonFormatter.SerializerSettings;
            settings.NullValueHandling = NullValueHandling.Ignore;
            settings.ContractResolver = new CamelCasePropertyNamesContractResolver();
            settings.Culture = new CultureInfo("de-DE");

            // Web API routes
            config.MapHttpAttributeRoutes();

            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "{locationId}/{controller}/{tappId}",
                constraints: new { locationId = new IntRouteConstraint() },
                defaults: new { id = RouteParameter.Optional }
                );
        }
    }
}
