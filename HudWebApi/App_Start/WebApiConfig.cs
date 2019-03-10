using System.Linq;
using System.Web.Http;
using System.Net.Http.Formatting;
using Newtonsoft.Json.Serialization;

namespace Hud.Application.Service
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            // Web API configuration and services

            // Web API routes
            config.MapHttpAttributeRoutes();

            config.Routes.MapHttpRoute(
                name: "ZipCodesApi",
                routeTemplate: "api/hud/zipcodes/{zip}",
                defaults: new { controller = "zipcodes", zip = RouteParameter.Optional }
            );

            config.Routes.MapHttpRoute(
                name: "ZipToCbsaMappingsApi",
                routeTemplate: "api/hud/mappings/zipToCbsa",
                defaults: new { controller = "ZipToCbsaMappings" }
            );

            config.Routes.MapHttpRoute(
                name: "CbsaToMsaMappingsApi",
                routeTemplate: "api/hud/mappings/cbsaToMsa",
                defaults: new { controller = "CbsaToMsaMappings" }
            );

            var jsonFormatter = config.Formatters.OfType<JsonMediaTypeFormatter>().FirstOrDefault();
            jsonFormatter.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
        }
    }
}
