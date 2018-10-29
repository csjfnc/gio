using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin;
using Website.Identity.Context;
using Website.Identity.Model;
using Website.Identity.MySQL;

namespace Website.Identity.Configuration
{
    /// <summary>
    /// Classe reponsavel pela configuracao do Identity
    /// </summary>
    public class ApplicationUserManager : UserManager<ApplicationUser>
    {
        public ApplicationUserManager(IUserStore<ApplicationUser> store) : base(store) { }

        public static ApplicationUserManager Create(IdentityFactoryOptions<ApplicationUserManager> options, IOwinContext context)
        {
            var manager = new ApplicationUserManager(new UserStore<ApplicationUser>(context.Get<IdentityContext>()));

            // Configurando a validacao de logica dos usernames usado pelo Identity
            manager.UserValidator = new UserValidator<ApplicationUser>(manager)
            {
                AllowOnlyAlphanumericUserNames = false,
                RequireUniqueEmail = false
            };

            // Configurando a validacao das senhas dos usuarios usado pelo Idenity
            manager.PasswordValidator = new PasswordValidator
            {
                RequiredLength = 6,
                RequireDigit = true,
            };

            //Configura se o usuario e bloqueado apos tentativas de senha incorreta.
            manager.UserLockoutEnabledByDefault = false;

            //Carregando o provider
            var dataProtectionProvider = options.DataProtectionProvider;
            if (dataProtectionProvider != null)
            {
                manager.UserTokenProvider = new DataProtectorTokenProvider<ApplicationUser>(dataProtectionProvider.Create("Visium 2k16"));
            }


            return manager;
        }
    }
}