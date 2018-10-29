using Microsoft.AspNet.Identity.Owin;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Website.DAL.UnitOfWork;
using Website.Identity.Configuration;
using Website.Identity.CustomAutorizes;
using Website.MVC.Helpers.CustomAttribute;
using Website.MVC.Helpers.CustomController;

namespace Website.MVC.Controllers
{
    public class HomeController : BaseController
    {
        private readonly UnitOfWork UnitOfWork = new UnitOfWork();

        [AppAuthorize(Modulos = new Modules[] { Modules.ORDEM_DE_SERVICO }, Permissoes = new Permissions[] { Permissions.CONSULTAR })]
        public ActionResult Index()
        {
            ViewBag.ListUsers = HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>().Users.ToList().OrderBy(u => u.UserName);            
            return View();
        }

        protected override void Dispose(bool disposing)
        {
            UnitOfWork.Dispose();
            base.Dispose(disposing);
        }
    }
}