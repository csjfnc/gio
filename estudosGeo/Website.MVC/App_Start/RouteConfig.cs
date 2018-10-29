using System.Web.Mvc;
using System.Web.Routing;
using Website.MVC.Helpers.Config;

namespace Website.MVC
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new {
                    controller = StringContantsWebsite.URI_START_PAGE_CONTROLLER,
                    action = StringContantsWebsite.URI_START_PAGE_ACTION,
                    id = UrlParameter.Optional
                }
            );
        }
    }
}