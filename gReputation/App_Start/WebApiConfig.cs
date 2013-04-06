using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Formatting;
using System.Web.Http;

namespace gReputation
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            //config.Formatters.JsonFormatter.("json", "application/json");
            //config.Formatters.XmlFormatter.AddUriPathExtensionMapping("xml", "text/xml");
            // TODO: read format based on extension
            config.Formatters.XmlFormatter.SupportedMediaTypes.Clear();
            config.Formatters.JsonFormatter.MediaTypeMappings.Add(
                new UriPathExtensionMapping("json", "application/json"));


            config.Routes.MapHttpRoute(
                name: "ActivityApi",
                routeTemplate: "{appName}/activity/{subjectId}/{actionName}/{objectId}",
                defaults: new { Controller = "ActivityApi", objectId = RouteParameter.Optional } //, ext = RouteParameter.Optional }
            );

            config.Routes.MapHttpRoute(
                name: "RulesApi",
                routeTemplate: "{appName}/rules/{id}",
                defaults: new { Controller = "RulesApi", id = RouteParameter.Optional } //, ext = RouteParameter.Optional }
            );

            config.Routes.MapHttpRoute(
                name: "ReputationApi",
                routeTemplate: "{appName}/{objectId}/reputation/{stat}",
                defaults: new { Controller = "ReputationApi", stat = RouteParameter.Optional } //, ext = RouteParameter.Optional }
            );

            config.Routes.MapHttpRoute(
                name: "ReputationApiGlobal",
                routeTemplate: "{objectId}/reputation/{stat}",
                defaults: new { Controller = "ReputationApi", stat = RouteParameter.Optional } //, ext = RouteParameter.Optional }
            );
        }
    }
}
