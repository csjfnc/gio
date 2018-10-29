using Microsoft.Owin;
using Owin;
using System.Web.Http;
using System.Web.Mvc;

[assembly: log4net.Config.XmlConfigurator(ConfigFile = "Web.config", Watch = true)]
[assembly: OwinStartup(typeof(Website.MVC.Startup))]
namespace Website.MVC
{
    public partial class Startup
    {
        public static HttpConfiguration HttpConfiguration { get; private set; }

        public void Configuration(IAppBuilder app)
        {
            HttpConfiguration = new HttpConfiguration();

            // Configuração do Identity
            ConfigureAuth(app);

            // Configuração do Web API (Webservice Mobile)
            ConfigureWebApi(app);

            AreaRegistration.RegisterAllAreas();
        }
    }
}