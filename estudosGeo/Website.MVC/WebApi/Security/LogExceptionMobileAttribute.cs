using log4net;
using System;
using System.Net;
using System.Net.Http;
using System.Web.Http.Filters;

namespace Website.MVC.WebApi.Security
{
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false)]
    public class LogExceptionMobileAttribute : ExceptionFilterAttribute
    {
        private readonly ILog Logger = LogManager.GetLogger("[Mobile]");

        public override void OnException(HttpActionExecutedContext context)
        {
            string CodeError = Guid.NewGuid().ToString().Substring(0, 8);
            Logger.Error(CodeError, context.Exception);

            /// Custom Error
            context.Response = context.Request.CreateResponse(
                HttpStatusCode.InternalServerError,
                new
                {
                    Type = "API Internal Error",
                    Message = string.Format(Resources.Messages.Error_Call_Mobile, CodeError)
                }
            );
        }
    }
}