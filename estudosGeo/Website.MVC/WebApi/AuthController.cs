using System.Net;
using System.Net.Http;
using System.Threading;
using System.Web.Http;
using WebApi.Security;
using Website.BLL.Entities;
using Website.DAL.UnitOfWork;
using Website.MVC.Helpers.Config;
using Website.MVC.WebApi.Models;

namespace Website.MVC.WebApi
{
    [AutenticateMobile]
    public class AuthController : ApiController
    {
        private readonly UnitOfWorkMobile UnitOfWorkMobile = new UnitOfWorkMobile();

        [HttpPost]
        public HttpResponseMessage Login()
        {
            if (Thread.CurrentPrincipal != null && Thread.CurrentPrincipal.Identity.IsAuthenticated)
            {
                MobileIdentity basicAuthenticationIdentity = Thread.CurrentPrincipal.Identity as MobileIdentity;
                if (basicAuthenticationIdentity != null)
                {
                    Mobile mobile = UnitOfWorkMobile.Repository.GenerateToken(basicAuthenticationIdentity.Imei, basicAuthenticationIdentity.IdUsuario);
                    HttpResponseMessage response = 
                        Request.CreateResponse(
                            HttpStatusCode.OK,
                            new ResponseApi() 
                            { 
                                Status = Status.OK,
                                Message = "Authorized",
                                Results = new 
                                {
                                    UserName = basicAuthenticationIdentity.Name,
                                    Login = basicAuthenticationIdentity.Login,
                                    Modules = basicAuthenticationIdentity.Modules
                                }
                            });
                    response.Headers.Add(StringContantsWebsite.TOKEN, mobile.AuthToken);                    
                    response.Headers.Add("Access-Control-Expose-Headers", StringContantsWebsite.TOKEN + "," + StringContantsWebsite.TOKEN_EXPIRY);

                    return response;
                }
            }
            return null;
        }
    }
}