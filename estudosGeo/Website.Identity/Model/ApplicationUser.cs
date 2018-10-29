using Microsoft.AspNet.Identity;
using System.Security.Claims;
using System.Threading.Tasks;
using Website.Identity.Configuration;
using Website.Identity.MySQL;

namespace Website.Identity.Model
{
    /// <summary>
    /// Classe responsavel pelo usuario do Identity e da App.
    /// </summary>
    public class ApplicationUser : IdentityUser
    {
        public ApplicationUser() { }

        public ClaimsIdentity GenerateUserIdentity(ApplicationUserManager manager)
        {
            /// Observe que o authenticationType precisa ser o mesmo que 
            /// foi definido em CookieAuthenticationOptions.AuthenticationType
            var userIdentity = manager.CreateIdentity(this, DefaultAuthenticationTypes.ApplicationCookie);

            return userIdentity;
        }

        public Task<ClaimsIdentity> GenerateUserIdentityAsync(ApplicationUserManager manager)
        {
            return Task.FromResult(GenerateUserIdentity(manager));
        }
    }
}