using System;
using System.Net;
using System.Net.Http;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;
using System.Linq;
using Website.MVC.Helpers.Config;
using Website.DAL.UnitOfWork;
using System.Configuration;

namespace Website.MVC.WebApi.Security
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = false)]
    public class AuthorizationMobileAttribute : ActionFilterAttribute
    {
        private UnitOfWorkMobile UnitOfWorkMobile = new UnitOfWorkMobile();

        private readonly bool _isActive = true;
        public AuthorizationMobileAttribute()
        {
            _isActive = ConfigurationManager.AppSettings.Get("AuthWebApiMobile") != null ? Convert.ToBoolean(ConfigurationManager.AppSettings.Get("AuthWebApiMobile")) : true;
        }

        public override void OnActionExecuting(HttpActionContext filterContext)
        {
            if (!_isActive)
            {
                base.OnActionExecuting(filterContext);
                return;
            }

            /// Verificando se existe o IMEI e TOKEN no Header do Request.
            if (filterContext.Request.Headers.Contains(StringContantsWebsite.IMEI) && filterContext.Request.Headers.Contains(StringContantsWebsite.TOKEN))
            {
                string imei = filterContext.Request.Headers.GetValues(StringContantsWebsite.IMEI).First();
                string token = filterContext.Request.Headers.GetValues(StringContantsWebsite.TOKEN).First();

                /// Validando a requisição do dispositivo.
                if (!UnitOfWorkMobile.Repository.ValidateImeiAndToken(imei, token))
                {
                    filterContext.Response = new HttpResponseMessage(HttpStatusCode.Unauthorized)
                    {
                        ReasonPhrase = Resources.Messages.Token_Expired
                    };
                }
            }
            else
            {
                filterContext.Response = new HttpResponseMessage(HttpStatusCode.Unauthorized) { ReasonPhrase = Resources.Messages.Imei_Or_Token_Not_Found };
            }

            base.OnActionExecuting(filterContext);
        }
    }
}