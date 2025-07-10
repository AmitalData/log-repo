using System.Web.Http;

namespace AmitalCloud.Shipment.WebAPI
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            config.Filters.Add(new AuthenticationExceptionFilter());

            config.MapHttpAttributeRoutes();
            //config.Routes.MapHttpRoute(
            //                            name: "ActionApi",
            //                            routeTemplate: "api/Infra/{controller}/{action}"
            //                            );

            config.Routes.MapHttpRoute(
                name: "API Default",
                routeTemplate: "api/{controller}/{action}/{id}",
                defaults: new { id = RouteParameter.Optional });
        }
    }
}
