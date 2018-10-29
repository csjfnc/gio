using System.Web.Http;
using System.Web.Http.Controllers;
using Website.BLL.Utils.Security;
using Website.DAL.UnitOfWork;
using Website.MVC.Helpers.Config;
using Website.MVC.WebApi.Security;
using System.Linq;
using System.Net.Http;
using Website.MVC.WebApi.Models;
using Website.BLL.Entities;
using System.Collections.Generic;
using System.Net;
using System.ComponentModel.DataAnnotations;
using Website.BLL.Utils.Geocoding;

namespace Website.MVC.WebApi
{
    [AuthorizationMobile]
    public class IPController : ApiController
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
        public HttpResponseMessage GetByPoste(long IdPoste)
        {
            IEnumerable<IP> IpsDB = UnitOfWork.IPRepository.Get(ip => ip.IdPoste == IdPoste && ip.DataExclusao == null);
            if (IpsDB != null)
            {
                return Request.CreateResponse(HttpStatusCode.OK, new ResponseApi() { Status = Status.OK, Results = IpsDB });
            }
            else
                return Request.CreateResponse(HttpStatusCode.OK, new ResponseApi() { Status = Status.NOK, Message = Resources.Messages.Not_Register_Data_Base });
        }

        [HttpGet]
        [LogExceptionMobile]
        public HttpResponseMessage Get(long IdIP)
        {
            IP IpDB = UnitOfWork.IPRepository.Get(ip => ip.IdIp == IdIP && ip.DataExclusao == null).FirstOrDefault();
            if (IpDB != null)
            {
                return Request.CreateResponse(HttpStatusCode.OK, new ResponseApi()
                {
                    Status = Status.OK,
                    Results =
                    new
                    {
                        IdIp = IpDB.IdIp,
                        IdPoste = IpDB.IdPoste,
                        TipoBraco = IpDB.TipoBraco,
                        TipoLuminaria = IpDB.TipoLuminaria,
                        QtdLuminaria = IpDB.QtdLuminaria,
                        TipoLampada = IpDB.TipoLampada,
                        Potencia = IpDB.Potencia,
                        Fase = IpDB.Fase,
                        Acionamento = IpDB.Acionamento,
                        LampadaAcesa = IpDB.LampadaAcesa,
                        QtdLampada = IpDB.QtdLampada
                    }
                });
            }
            else
                return Request.CreateResponse(HttpStatusCode.OK, new ResponseApi() { Status = Status.NOK, Message = Resources.Messages.Not_Register_Data_Base });
        }

        [HttpPut]
        [LogExceptionMobile]
        public HttpResponseMessage Edit(IPEditAPI IP)
        {
            /// Validando o IP
            ICollection<ValidationResult> results;
            if (IP.TryValidate(out results))
            {
                IP IpDB = UnitOfWork.IPRepository.Get(ip => ip.IdIp == IP.IdIp && ip.DataExclusao == null, includeProperties: "Poste.Cidade").FirstOrDefault();
                if (IpDB != null)
                {
                    //Atribuindo os novos valores da Edicao
                    IpDB.TipoBraco = IP.TipoBraco;
                    IpDB.TipoLuminaria = IP.TipoLuminaria;
                    IpDB.QtdLuminaria = IP.QtdLuminaria;
                    IpDB.TipoLampada = IP.TipoLampada;
                    IpDB.Potencia = IP.Potencia;
                    IpDB.Fase = IP.Fase;
                    IpDB.Acionamento = IP.Acionamento;
                    IpDB.LampadaAcesa = IP.LampadaAcesa;
                    IpDB.QtdLampada = IP.QtdLampada;

                    // Salvando a posição do Mobile no momento da Edição do poste
                    UTM posicaoAtualizacao = new ConverterLatLonToUtm(IpDB.Poste.Cidade.Datum, IpDB.Poste.Cidade.NorteOuSul, IpDB.Poste.Cidade.Zona).Convert(IP.LatAtualizacao, IP.LonAtualizacao);
                    IpDB.Poste.XAtualizacao = posicaoAtualizacao.X;
                    IpDB.Poste.YAtualizacao = posicaoAtualizacao.Y;

                    UnitOfWork.IPRepository.Update(IpDB);
                    UnitOfWork.Save(); //Commit

                    return Request.CreateResponse(HttpStatusCode.OK, new ResponseApi()
                    {
                        Status = Status.OK,
                        Message = Resources.Messages.Save_OK,
                        Results = new
                        {
                            IdIp = IpDB.IdIp,
                            IdPoste = IpDB.IdPoste,
                            TipoBraco = IpDB.TipoBraco,
                            TipoLuminaria = IpDB.TipoLuminaria,
                            QtdLuminaria = IpDB.QtdLuminaria,
                            TipoLampada = IpDB.TipoLampada,
                            Potencia = IpDB.Potencia,
                            Fase = IpDB.Fase,
                            Acionamento = IpDB.Acionamento,
                            LampadaAcesa = IpDB.LampadaAcesa,
                            QtdLampada = IpDB.QtdLampada
                        }
                    });
                }
                else
                    return Request.CreateResponse(HttpStatusCode.OK, new ResponseApi() { Status = Status.NOK, Message = Resources.Messages.Ip_Not_Found });
            }
            else
            {
                string MessageValidate = string.Empty;
                foreach (var validationResult in results) MessageValidate += validationResult.ErrorMessage + "-";
                return Request.CreateResponse(HttpStatusCode.OK, new ResponseApi() { Status = Status.NOK, Message = MessageValidate.Remove(MessageValidate.Length - 1) });
            }
        }

        [HttpPost]
        [LogExceptionMobile]
        public HttpResponseMessage Add(IPEditAPI IP)
        {
            /// Validando o IP
            ICollection<ValidationResult> results;
            if (IP.TryValidate(out results))
            {
                Poste PosteBD = UnitOfWork.PosteRepository.Get(os => os.IdPoste == IP.IdPoste, includeProperties: "Cidade").FirstOrDefault();
                if (PosteBD != null)
                {
                    // Salvando a posição do Mobile no momento da Edição do poste
                    UTM posicaoAtualizacao = new ConverterLatLonToUtm(PosteBD.Cidade.Datum, PosteBD.Cidade.NorteOuSul, PosteBD.Cidade.Zona).Convert(IP.LatAtualizacao, IP.LonAtualizacao);
                    PosteBD.XAtualizacao = posicaoAtualizacao.X;
                    PosteBD.YAtualizacao = posicaoAtualizacao.Y;

                    IP IPAux = new IP()
                    {
                        Poste = PosteBD,
                        TipoBraco = IP.TipoBraco,
                        TipoLuminaria = IP.TipoLuminaria,
                        QtdLuminaria = IP.QtdLuminaria,
                        TipoLampada = IP.TipoLampada,
                        Potencia = IP.Potencia,
                        CodigoGeoBD = -1,
                        Acionamento = IP.Acionamento,
                        LampadaAcesa = IP.LampadaAcesa,
                        Fase = IP.Fase,
                        QtdLampada = IP.QtdLampada
                    };

                    UnitOfWork.IPRepository.Insert(IPAux);
                    UnitOfWork.Save();

                    return Request.CreateResponse(HttpStatusCode.OK, new ResponseApi()
                    {
                        Status = Status.OK,
                        Message = Resources.Messages.Save_OK,
                        Results = new
                        {
                            IdIp = IPAux.IdIp,
                            IdPoste = IPAux.IdPoste,
                            TipoBraco = IPAux.TipoBraco,
                            TipoLuminaria = IPAux.TipoLuminaria,
                            QtdLuminaria = IPAux.QtdLuminaria,
                            TipoLampada = IPAux.TipoLampada,
                            Potencia = IPAux.Potencia,
                            Fase = IPAux.Fase,
                            Acionamento = IPAux.Acionamento,
                            LampadaAcesa = IPAux.LampadaAcesa,
                            QtdLampada = IPAux.QtdLampada
                        }
                    });
                }
                else
                    return Request.CreateResponse(HttpStatusCode.OK, new ResponseApi() { Status = Status.NOK, Message = Resources.Messages.Poste_Not_Found });
            }
            else
            {
                string MessageValidate = string.Empty;
                foreach (var validationResult in results) MessageValidate += validationResult.ErrorMessage;
                return Request.CreateResponse(HttpStatusCode.OK, new ResponseApi() { Status = Status.NOK, Message = MessageValidate });
            }
        }
    }
}