using System;
using System.Net;
using System.Net.Http;
using System.Security.Principal;
using System.Text;
using System.Threading;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;
using System.Web;
using System.Net.Http.Headers;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.AspNet.Identity;
using Website.Identity.Model;
using Website.Identity.Configuration;
using Website.MVC.Helpers.Config;
using System.Linq;
using Website.DAL.UnitOfWork;
using Website.Identity.CustomAutorizes;

namespace WebApi.Security
{
    /// <summary>
    /// Filtro para a Autenticação dos dispositivos mobiles.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = false)]
    public class AutenticateMobileAttribute : AuthorizationFilterAttribute
    {
        private readonly UnitOfWorkMobile UnitOfWorkMobile = new UnitOfWorkMobile();

        public override void OnAuthorization(HttpActionContext filterContext)
        {
            MobileIdentity identity = isRequestValid(filterContext);
            if (identity == null)
            {
                InvalidateRequest(filterContext);
                return;
            }

            Thread.CurrentPrincipal = OnAuthorizeUser(identity, filterContext);
            if (!Thread.CurrentPrincipal.Identity.IsAuthenticated)
            {
                InvalidateRequest(filterContext);
                return;
            }

            base.OnAuthorization(filterContext);
        }

        #region Regras de Validação

        /// <summary>
        /// Validando o usuário no banco de dados
        /// </summary>
        /// <param name="currentPrincipal"></param>
        /// <param name="identity"></param>
        /// <param name="filterContext"></param>
        /// <returns></returns>
        private IPrincipal OnAuthorizeUser(MobileIdentity identity, HttpActionContext filterContext)
        {
            ApplicationUser user = HttpContext.Current.GetOwinContext().GetUserManager<ApplicationUserManager>().Find(identity.Login, identity.Password);
            if (user != null)
            {
                identity.IdUsuario = user.Id;
                identity.Modules = user.Claims.Where(p => p.ClaimType != Permissions.PERMISSAO.GetString()).Select(c => c.ClaimValue).ToList();
                return new GenericPrincipal(identity, null);
            }
            else
                return Thread.CurrentPrincipal;
        }

        /// <summary>
        /// Vetifica se a requisição é de um dispotivo cadastrado no sistema.
        /// </summary>
        /// <param name="filterContext"></param>
        /// <returns></returns>
        protected virtual MobileIdentity isRequestValid(HttpActionContext filterContext)
        {
            try
            {
                /// Check if IMEI exist into header
                if (!(filterContext.Request.Headers.Contains(StringContantsWebsite.IMEI) && UnitOfWorkMobile.Repository.ExistImei(filterContext.Request.Headers.GetValues(StringContantsWebsite.IMEI).FirstOrDefault())))
                    return null;

                string CREDENTIALS = null;
                AuthenticationHeaderValue authRequest = filterContext.Request.Headers.Authorization;
                if (authRequest != null && !string.IsNullOrEmpty(authRequest.Scheme) && authRequest.Scheme == "Basic")
                    CREDENTIALS = authRequest.Parameter;

                /// Check Credentials user
                if (string.IsNullOrEmpty(CREDENTIALS))
                    return null;

                CREDENTIALS = Encoding.Default.GetString(Convert.FromBase64String(CREDENTIALS));
                string[] credentials = CREDENTIALS.Split(':');
                string imei = filterContext.Request.Headers.GetValues(StringContantsWebsite.IMEI).FirstOrDefault();
                
                ////*Alterações*////
                string version = credentials[2];
                AtualizaVersionAplicativo(imei,version);
                ////*Alterações*////

                return credentials.Length < 2 ? null : new MobileIdentity(credentials[0], credentials[1], imei);
            }
            catch
            {
                return null;
            }
        }

        private static void InvalidateRequest(HttpActionContext filterContext)
        {
            filterContext.Response = new HttpResponseMessage(HttpStatusCode.Unauthorized);
            filterContext.Response.Headers.Add("WWW-Authenticate", string.Format("Basic realm=\"{0}\"", filterContext.Request.RequestUri.DnsSafeHost));
        }

        ////Vou Implementar Temporariamente aqui 
        ////Ate analizar o local ideal para ficar esse Codigo
        private void AtualizaVersionAplicativo(string imei, string version) 
        {
            Website.BLL.Entities.Mobile cell = UnitOfWorkMobile.Repository.GetByImei(imei);
            cell.Version = version;
            UnitOfWorkMobile.Repository.Update(cell);
        }

        #endregion
    }
}