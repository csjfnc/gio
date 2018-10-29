using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Website.MVC.Controllers
{
    public class ErrorController : Controller
    {
        [AllowAnonymous]
        public ActionResult ErrorPage()
        {
            //Atribuindo a Mensagem de Erro da View
            ViewBag.Error = (TempData["Error"] as Exception) != null ? (TempData["Error"] as Exception).Message : null;
          
            return View();
        }

        [AllowAnonymous]
        public ActionResult Error404()
        {
            return View();
        }
    }
}