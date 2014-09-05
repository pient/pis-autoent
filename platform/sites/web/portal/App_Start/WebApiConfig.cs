using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;

namespace PIS.AutoEnt.Portal.WebSite
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{action}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );

            config.Routes.MapHttpRoute(
                name: "SetupApi",
                routeTemplate: "api/setup/{controller}/{action}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );

            config.Filters.Add(new Filters.ExceptionFilter());
        }
    }
}
