using log4net;
using System;
using System.Web.Mvc;

namespace Website.MVC.Helpers.CustomController
{
    public class BaseController : Controller
    {
        /// Logger
        private readonly ILog Logger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        #region Customização dos erros do website

        /// <summary>
        /// Tratando as Exceptions não tratadas no Controller
        /// </summary>
        /// <param name="filterContext"></param>
        protected override void OnException(ExceptionContext filterContext)
        {
            string CodeError = Guid.NewGuid().ToString().Substring(0, 8);

            /// Grava a trace da Exception no arquivo Log de erro.
            if (!(filterContext.Exception.GetType() == typeof(HttpAntiForgeryException)))
            {
                Logger.Error("[" + CodeError + "]" + filterContext.Exception.Message, filterContext.Exception);
            }
            
            /// Se o request for AJAX, retorne um JSON com as mensagens de erro.
            /// Se não o request redireciona para a página de erro default.
            if (filterContext.HttpContext.Request.IsAjaxRequest())
            {
                string Msg = "Code: [" + CodeError + "]" + Resources.Messages.Error_Call_Ajax;
                int StatusCode = filterContext.HttpContext.Response.StatusCode;
                filterContext.Result = Json(new { StatusCode = StatusCode, Msg = Msg } , JsonRequestBehavior.AllowGet);
            }
            else
            {
                TempData["Error"] = filterContext.Exception;
                filterContext.ExceptionHandled = true;
                filterContext.Result = RedirectToAction("ErrorPage", "Error");
            }

            base.OnException(filterContext);
        }

        #endregion
    }
}