using System.Web;
using System.Web.Optimization;

namespace Website.MVC
{
    public class BundleConfig
    {
        // For more information on bundling, visit http://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                        "~/Scripts/jquery-{version}.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                        "~/Scripts/jquery.validate*"));

            // Use the development version of Modernizr to develop with and learn from. Then, when you're
            // ready for production, use the build tool at http://modernizr.com to pick only the tests you need.
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Scripts/modernizr-*"));

            bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
                      "~/Scripts/bootstrap.js",
                      "~/Scripts/respond.js"));

            bundles.Add(new StyleBundle("~/Content/css").Include(
                      "~/Content/bootstrap.css",
                      "~/Content/site.css", "~/Content/login.css"));

            bundles.Add(new StyleBundle("~/Content/datatable").Include(
                      "~/Content/dataTables.bootstrap.min.css",
                      "~/Content/buttons.dataTables.min.css"));

            //Bootstrap Modal Custom
            bundles.Add(new StyleBundle("~/Content/ModalCustom").Include(
                        "~/Content/ModalCustom.css"));



            bundles.Add(new ScriptBundle("~/bundles/datatable").Include(
                        "~/Scripts/jquery.dataTables.min.js",
                        "~/Scripts/dataTables.bootstrap.min.js",
                        "~/Scripts/dataTables.buttons.min.js",
                        "~/Scripts/jszip.min.js",
                        "~/Scripts/pdfmake.min.js",
                        "~/Scripts/vfs_fonts.js",
                        "~/Scripts/buttons.html5.min.js"
                      ));


            bundles.Add(new ScriptBundle("~/bundles/maplabel-compiled.js").Include("~/Scripts/maplabel-compiled.js"));
            bundles.Add(new ScriptBundle("~/bundles/richmarker-compiled.js").Include("~/Scripts/richmarker-compiled.js"));

            //Lib de Criação do Mapa e Manipulação das Ordens de Serviço
            bundles.Add(new ScriptBundle("~/bundles/GlobalMaps").Include("~/Scripts/global.Maps.js", "~/Scripts/richmarker-compiled.js","~/Scripts/richmarker-compiled.js"));

            //Lib de Criação do Mapa e Manipulação das Redes de Poste, bootstrap BootBox
            bundles.Add(new ScriptBundle("~/bundles/RedeMaps").Include(
                    "~/Scripts/jquery.mask.js",
                    "~/Scripts/bootbox.min.js",
                    "~/Scripts/rede.Maps-1.2.js"));

            //Usuarios
            bundles.Add(new ScriptBundle("~/bundles/Usuario").Include(
                "~/Scripts/jquery.dataTables.min.js",
                "~/Scripts/dataTables.bootstrap.min.js",
                "~/Scripts/bootbox.min.js",
                "~/Scripts/Usuarios.js"));

            //Bootstrap Modal Custom
            bundles.Add(new ScriptBundle("~/bundles/ModalCustom").Include("~/Scripts/ModalCustom.js"));

            //Bootstrap BootBox
            bundles.Add(new ScriptBundle("~/bundles/BootBox").Include("~/Scripts/bootbox.min.js"));

            //Pace Loading Default css
            bundles.Add(new StyleBundle("~/Content/Pace").Include("~/Content/pace.css"));

            //Pace Loading Maps css
            bundles.Add(new StyleBundle("~/Content/PaceMaps").Include("~/Content/pace-maps.css"));
            bundles.Add(new StyleBundle("~/Content/Desenhos").Include("~/Content/desenhos.css"));
        }
    }
}
