using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;
using Website.MVC.WebApi.Models;

namespace Website.MVC.WebApi.OfflineMode
{
    public class MimeMultipartAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(HttpActionContext actionContext)
        {
            if (!actionContext.Request.Content.IsMimeMultipartContent())
            {
                actionContext.Response = actionContext.Request.CreateResponse(
                    HttpStatusCode.InternalServerError, 
                    new ResponseApi() { Status = Status.NOK, Message = Resources.Messages.Error_Upload_MimeMultipart }
                );
            }
        }
    }
}