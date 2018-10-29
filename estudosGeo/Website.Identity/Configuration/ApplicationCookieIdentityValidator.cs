using System;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security.Cookies;
using Website.Identity.Model;

namespace Website.Identity.Configuration
{
    /// <summary>
    /// Validação do Secutiry Stamp para usuario conectado nos clients registrados.
    /// </summary>
    public static class ApplicationCookieIdentityValidator
    {
        private static async Task<bool> VerifySecurityStampAsync(ApplicationUserManager manager, ApplicationUser user, CookieValidateIdentityContext context)
        {
            string stamp = context.Identity.FindFirstValue("AspNet.Identity.SecurityStamp");
            return (stamp == await manager.GetSecurityStampAsync(context.Identity.GetUserId()));
        }

        public static Func<CookieValidateIdentityContext, Task> OnValidateIdentity(TimeSpan validateInterval, Func<ApplicationUserManager, ApplicationUser, Task<ClaimsIdentity>> regenerateIdentity)
        {
            return async context =>
            {
                DateTimeOffset utcNow = context.Options.SystemClock.UtcNow;
                DateTimeOffset? issuedUtc = context.Properties.IssuedUtc;
                bool expired = false;
                if (issuedUtc.HasValue)
                {
                    TimeSpan t = utcNow.Subtract(issuedUtc.Value);
                    expired = (t > validateInterval);
                }
                if (expired)
                {
                    var userManager = context.OwinContext.GetUserManager<ApplicationUserManager>();
                    string userId = context.Identity.GetUserId();

                    if (userManager != null && userId != null)
                    {
                        var user = await userManager.FindByIdAsync(userId);
                        bool reject = true;
                        if (user != null && await VerifySecurityStampAsync(userManager, user, context))
                        {
                            reject = false;
                            if (regenerateIdentity != null)
                            {
                                ClaimsIdentity claimsIdentity = await regenerateIdentity(userManager, user);
                                if (claimsIdentity != null)
                                {
                                    context.OwinContext.Authentication.SignIn(new ClaimsIdentity[]
                                    {
                                        claimsIdentity
                                    });
                                }
                            }
                        }
                        if (reject)
                        {
                            context.RejectIdentity();
                            context.OwinContext.Authentication.SignOut(new string[]
                            {
                                context.Options.AuthenticationType
                            });
                        }
                    }
                }
            };
        }
    }
}