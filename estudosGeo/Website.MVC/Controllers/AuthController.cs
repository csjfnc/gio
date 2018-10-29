using Microsoft.AspNet.Identity.Owin;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Website.Identity.Configuration;
using Website.MVC.Helpers.CustomController;
using Website.MVC.Models;

namespace Website.MVC.Controllers
{
    public class AuthController : BaseController
    {
        [HttpGet]
        [AllowAnonymous]
        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Login(LoginViewModel model)
        {
            /// Verificando se os dados passados estão corretos.
            if (!ModelState.IsValid) return View(model);

            ApplicationSignInManager appManager = HttpContext.GetOwinContext().Get<ApplicationSignInManager>();
            SignInStatus result = await appManager.PasswordSignInAsync(model.Login, model.Password, false, shouldLockout: false);
            switch (result)
            {
                case SignInStatus.Success:
                    return RedirectToAction("Index", "Home");
                case SignInStatus.Failure:
                    ModelState.AddModelError("Error", Resources.Messages.User_Or_Passord_Incorrect);                    
                    return View(model);
                default:
                    ModelState.AddModelError("Error", Resources.Messages.Failure_Sign);
                    return View(model);
            }
        }

        [HttpGet]
        [Authorize]
        public ActionResult Logout()
        {
            HttpContext.GetOwinContext().Authentication.SignOut();
            return RedirectToAction("Login", "Auth");
        }
    }
}