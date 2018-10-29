using System.Net;
using System.Net.Http;
using System.Web.Http;
using Website.DAL.UnitOfWork;
using System.Linq;
using Website.BLL.Entities;
using System.Collections.Generic;
using Website.BLL.Utils.Geocoding;
using Website.MVC.WebApi.Security;
using Website.MVC.Helpers.Config;
using System.Web.Http.Controllers;
using Website.BLL.Utils.Security;
using Website.MVC.WebApi.Models;
using System;
using Website.MVC.WebApi.Bussiness;

namespace Website.MVC.WebApi
{
    [AuthorizationMobile]
    public class OSController : ApiController
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
        public HttpResponseMessage GetByCidade(long idCidade)
        {
            IEnumerable<OrdemDeServico> OrdensDeServico = UnitOfWork.OrdemDeServicoRepository.Get(os => os.IdUsuario == IdUsuario && os.Cidade.IdCidade == idCidade, includeProperties: "Cidade,PoligonosOS");
            if (OrdensDeServico != null && OrdensDeServico.Count() > 0)
            {
                /// Criando a lista de Ordem de Servico para o Mobile
                ConverterUtmToLatLon converter = null;
                List<object> jsonOrdemDeServico = new List<object>();
                foreach (OrdemDeServico OS in OrdensDeServico)
                {
                    if (converter == null)
                        converter = new ConverterUtmToLatLon(OS.Cidade.Datum, OS.Cidade.NorteOuSul, OS.Cidade.Zona);

                    /// Pontos do poligono
                    List<LatLon> PoligonosOs = OS.PoligonosOS.OrderBy(o => o.Ordem).Select(os => converter.Convert(os.X1, os.Y1)).ToList();

                    /// Objeto que será serializado para json
                    jsonOrdemDeServico.Add(new { IdOrdemDeServico = OS.IdOrdemDeServico, NumeroOS = OS.NumeroOS, PoligonosOS = PoligonosOs, Status = CalculaStatusOs.Calcular(OS) });
                }

                return Request.CreateResponse(HttpStatusCode.OK, new ResponseApi() { Status = Status.Found, Message = Resources.Messages.Found_Register, Results = jsonOrdemDeServico });
            }
            else
                return Request.CreateResponse(HttpStatusCode.OK, new ResponseApi() { Status = Status.NotFound, Message = Resources.Messages.Not_Register_Data_Base });
        }

        [HttpGet]
        [LogExceptionMobile]
        public HttpResponseMessage Get(long IdOrdemDeServico)
        {
            OrdemDeServico OS = UnitOfWork.OrdemDeServicoRepository.Get(os => os.IdOrdemDeServico == IdOrdemDeServico, includeProperties: "Usuario, Postes").FirstOrDefault();
            if (OS != null)
            {
                return Request.CreateResponse(HttpStatusCode.OK, new ResponseApi()
                {
                    Status = Status.Found,
                    Message = Resources.Messages.Found_Register,
                    Results = new { IdOrdemDeServico = OS.IdOrdemDeServico, NumeroOS = OS.NumeroOS, DataInicio = OS.DataInicio, DataFinal = OS.DataFinal, Usuario = OS.Usuario.UserName, NumeroDePostes = OS.Postes.Where(p => p.DataExclusao == null).Count() }
                });
            }
            else
                return Request.CreateResponse(HttpStatusCode.OK, new ResponseApi() { Status = Status.NotFound, Message = Resources.Messages.Not_Register_Data_Base });
        }

        [HttpPost]
        [LogExceptionMobile]
        public HttpResponseMessage Reabrir(long IdOrdemDeServico)
        {
            OrdemDeServico OS = UnitOfWork.OrdemDeServicoRepository.Get(os => os.IdOrdemDeServico == IdOrdemDeServico).FirstOrDefault();
            if (OS != null)
            {
                OS.DataFinal = null;
                UnitOfWork.OrdemDeServicoRepository.Update(OS);
                UnitOfWork.Save();

                return Request.CreateResponse(HttpStatusCode.OK, new ResponseApi() { Status = Status.OK, Message = Resources.Messages.Save_OK });
            }
            else
                return Request.CreateResponse(HttpStatusCode.OK, new ResponseApi() { Status = Status.NOK, Message = Resources.Messages.Not_Register_Data_Base });
        }

        [HttpPost]
        [LogExceptionMobile]
        public HttpResponseMessage Encerramento(long IdOrdemDeServico, DateTime Data)
        {
            OrdemDeServico OS = UnitOfWork.OrdemDeServicoRepository.Get(os => os.IdOrdemDeServico == IdOrdemDeServico).FirstOrDefault();
            if (OS != null)
            {
                OS.DataFinal = Data;
                UnitOfWork.OrdemDeServicoRepository.Update(OS);
                UnitOfWork.Save();

                return Request.CreateResponse(HttpStatusCode.OK, new ResponseApi() { Status = Status.OK, Message = Resources.Messages.Save_OK });
            }
            else
                return Request.CreateResponse(HttpStatusCode.OK, new ResponseApi() { Status = Status.NOK, Message = Resources.Messages.Not_Register_Data_Base });
        }
    }
}