using System.Globalization;
using System.Net.Http.Headers;
using System.Web.Http;
using Newtonsoft.Json.Serialization;
using System.Web.Http.Cors;

namespace EventRegistration
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            //Sets the media type for the response
            config.Formatters.JsonFormatter.SupportedMediaTypes.Add(new MediaTypeHeaderValue("text/html"));
            var cors = new EnableCorsAttribute("*", "*", "*");
            config.EnableCors(cors);
            //Response settings
            var settings = config.Formatters.JsonFormatter.SerializerSettings;
            //All Attributes should begin with lower case
            settings.ContractResolver = new CamelCasePropertyNamesContractResolver(); 
            settings.Culture = new CultureInfo("de-DE");
            // Web API routes
            config.MapHttpAttributeRoutes();

            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );
        }
    }
}
