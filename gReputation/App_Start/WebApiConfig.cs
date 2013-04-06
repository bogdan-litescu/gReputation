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
                name: "ReputationApi",
                routeTemplate: "{appName}/{userId}/{action}/{objectId}",
                defaults: new { Controller = "ReputationApi" } //, ext = RouteParameter.Optional }
            );

        }
    }
}
