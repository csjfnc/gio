using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Controllers;
using Website.BLL.Entities;
using Website.BLL.Utils.Security;
using Website.DAL.UnitOfWork;
using Website.MVC.Helpers.Config;
using Website.MVC.WebApi.Models;
using Website.MVC.WebApi.Security;

namespace Website.MVC.WebApi
{
    [AuthorizationMobile]
    public class CidadeController : ApiController
    {
        private readonly UnitOfWork UnitOfWork = new UnitOfWork();
        private string IdUsuario = string.Empty;

        protected override void Initialize(HttpControllerContext context)
        {
            if (context.Request.Headers.GetValues(StringContantsWebsite.TOKEN).First() != null)
            {
                IdUsuario = AESCrypt.Decrypt(context.Request.Headers.GetValues(StringContantsWebsite.TOKEN).First()).Split(':')[0];
            }

            base.Initialize(context);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (UnitOfWork != null)
                {
                    UnitOfWork.Dispose();
                }
            }
            base.Dispose(disposing);
        }

        [HttpGet]
        [LogExceptionMobile]
        public HttpResponseMessage GetList()
        {
            IEnumerable<Cidade> Cidades = UnitOfWork.CidadeRepository.Get();
            if (Cidades != null && Cidades.Count() > 0)
                return Request.CreateResponse(HttpStatusCode.OK, new ResponseApi() { Status = Status.Found, Message = Resources.Messages.Found_Register, Results = Cidades.Select(c => new { IdCidade = c.IdCidade, Nome = c.Nome }) });
            else
                return Request.CreateResponse(HttpStatusCode.OK, new ResponseApi() { Status = Status.NotFound, Message = Resources.Messages.Not_Register_Data_Base });
        }
    }
}