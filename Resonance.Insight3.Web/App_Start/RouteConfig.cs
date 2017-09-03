using System.Web.Mvc;
using System.Web.Routing;

namespace Resonance.Insight3.Web.App_Start
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(null, "Home", new {controller = "Home", action = "Home"});

            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new {controller = "Authentication", action = "Login", id = UrlParameter.Optional});
        }
    }
}