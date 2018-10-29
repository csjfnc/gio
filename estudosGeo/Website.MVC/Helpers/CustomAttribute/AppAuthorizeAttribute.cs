using System.Security.Claims;
using System.Threading;
using System.Web.Mvc;
using System.Web.Helpers;
using Website.Identity.CustomAutorizes;

namespace Website.MVC.Helpers.CustomAttribute
{
    public class AppAuthorizeAttribute : AuthorizeAttribute
    {
        public Modules[] Modulos;
        public Permissions[] Permissoes;
        public bool RequestVerificationToken = true;

        public override void OnAuthorization(AuthorizationContext context)
        {
            /// Verifica se a Requisição pertence a um usuario Autenticado no sistema.
            if (Thread.CurrentPrincipal.Identity.IsAuthenticated)
            {
                /// Regras de segurança para as chamdas Ajax do WebSite
                if (context.HttpContext.Request.IsAjaxRequest() && RequestVerificationToken)
                {
                    var antiForgeryCookie = context.HttpContext.Request.Cookies[AntiForgeryConfig.CookieName];
                    var cookieValue = antiForgeryCookie != null ? antiForgeryCookie.Value : null;
                    AntiForgery.Validate(cookieValue, context.HttpContext.Request.Headers["__RequestVerificationToken"]);
                }

                #region Validação dos Módulos e Grupos

                ClaimsIdentity UserIdentity = (ClaimsIdentity)Thread.CurrentPrincipal.Identity;                
                
                /// Verificando quando é passado os dois parâmetro para o Filtro.
                /// Parâmetros: (Modulos,Permissoes)
                if (Modulos != null && Permissoes != null)
                {
                    if (CheckModulo(UserIdentity) && CheckPermissoes(UserIdentity))
                    {
                        base.OnAuthorization(context);
                    }
                    else
                        HandleUnauthorizedRequest(context);
                }
                else if (Modulos != null) /// Quando houve apenas o parâmetro Modulos
                {
                    if (CheckModulo(UserIdentity))
                    {
                        base.OnAuthorization(context);
                    }
                    else
                        HandleUnauthorizedRequest(context);
                }
                else if (Permissoes != null) /// Quando houve apenas o parâmetro Permissoes     
                {
                    if (CheckPermissoes(UserIdentity))
                    {
                        base.OnAuthorization(context);
                    }
                    else
                        HandleUnauthorizedRequest(context);
                }

                #endregion
            }
            else
            {
                base.OnAuthorization(context);
            }
        }
                       
        protected override void HandleUnauthorizedRequest(AuthorizationContext context)
        {
            /// Regras de segurança para as chamdas Ajax do WebSite
            if (context.HttpContext.Request.IsAjaxRequest())
            {
                UrlHelper urlHelper = new UrlHelper(context.RequestContext);
                int StatusCode = context.HttpContext.Response.StatusCode = 403;
                string Msg = context.HttpContext.User.Identity.IsAuthenticated ? Resources.Messages.Not_Authorized_Ajax : Resources.Messages.Not_Autenticated_Ajax;

                context.Result = new JsonResult
                {
                    Data = new { StatusCode = StatusCode, Msg = Msg, RedirectTo = urlHelper.Action("Login", "Auth") },
                    JsonRequestBehavior = JsonRequestBehavior.AllowGet
                };
            }
            else
            {
                /// Continuando a validação padrão de segurança para as Views do WebSite.
                base.HandleUnauthorizedRequest(context);
            }
        }

        #region Validação dos parametros passados para o Filtro

        /// <summary>
        /// Verifica se o usuário possui o módulo passado para o Filtro.
        /// </summary>
        /// <param name="userIdentity"></param>
        /// <returns></returns>
        private bool CheckModulo(ClaimsIdentity userIdentity)
        {
            bool HasModulo = false;
            foreach (Modules m in Modulos)
            {
                HasModulo = userIdentity.HasClaim(Modules.MODULO.GetString(), m.GetString()); break;
            }

            return HasModulo;
        }

        /// <summary>
        /// Verifica se o usuário possui a permissão passada para o Filtro.
        /// </summary>
        /// <param name="userIdentity"></param>
        /// <returns></returns>
        private bool CheckPermissoes(ClaimsIdentity userIdentity)
        {
            bool HasPermissoes = false;
            foreach (Permissions p in Permissoes)
            {
                HasPermissoes = userIdentity.HasClaim(Permissions.PERMISSAO.GetString(), p.GetString()); break;
            }

            return HasPermissoes;
        }
        
        #endregion
    }
}