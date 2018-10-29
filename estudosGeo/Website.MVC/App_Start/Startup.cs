using Microsoft.AspNet.Identity;
using Microsoft.Owin;
using Microsoft.Owin.Security;
using Microsoft.Owin.Security.Cookies;
using Owin;
using System;
using System.Web.Http;
using Website.Identity.Configuration;
using Website.Identity.Context;
using Website.MVC.Helpers.Config;

namespace Website.MVC
{
    public partial class Startup
    {
        #region Configuração de inicialização do Identity

        public void ConfigureAuth(IAppBuilder app)
        {
            /// Configurando as Intancias do Idenity no momento em que a aplicacao
            /// for iniciada. ** Note que e necessario esta funcionalidade para que 
            /// o Identity funcione na Aplicacao.
            app.CreatePerOwinContext(IdentityContext.Create);
            app.CreatePerOwinContext<ApplicationUserManager>(ApplicationUserManager.Create);
            app.CreatePerOwinContext<ApplicationRoleManager>(ApplicationRoleManager.Create);
            app.CreatePerOwinContext<ApplicationSignInManager>(ApplicationSignInManager.Create);

            /// Configurando o modo de autenticacao usada pelo Identity
            /// ** Note que no 'provider' usado o validateInterval para ser
            /// validado a cada request, garantindo caso houver uma mudança
            /// de permissao ou senha do usuario seja aplicada na proxima 
            /// requisicao do usuario.
            app.UseCookieAuthentication(new CookieAuthenticationOptions
            {
                AuthenticationMode = AuthenticationMode.Active,
                AuthenticationType = DefaultAuthenticationTypes.ApplicationCookie,
                LoginPath = new PathString(StringContantsWebsite.URI_LOGIN),
                Provider = new CookieAuthenticationProvider
                {
                    OnValidateIdentity = ApplicationCookieIdentityValidator.OnValidateIdentity(
                    validateInterval: TimeSpan.FromMinutes(10),
                    regenerateIdentity: (manager, user) => user.GenerateUserIdentityAsync(manager)),
                    OnApplyRedirect = context =>
                    {
                        /// Verificando se deve ser redirecionado o contexto caso não seja via WebApi
                        if (!context.Request.Path.StartsWithSegments(new PathString("/" + StringContantsWebsite.URI_WEBAPI)))
                            context.Response.Redirect(context.RedirectUri);
                    }
                }
            });
        }

        #endregion

        #region Configuração da WebApi para os dispositivos Mobile

        public void ConfigureWebApi(IAppBuilder app)
        {
            HttpConfiguration.Routes.MapHttpRoute(
                name: "ServicoWebMobile",
                routeTemplate: StringContantsWebsite.URI_WEBAPI + "/{controller}/{action}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );

            app.UseWebApi(HttpConfiguration);
        }

        #endregion
    }
}