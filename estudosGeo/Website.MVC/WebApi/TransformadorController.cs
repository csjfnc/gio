using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Controllers;
using Website.BLL.Entities;
using Website.BLL.Utils.Geocoding;
using Website.BLL.Utils.Security;
using Website.DAL.UnitOfWork;
using Website.MVC.Helpers.Config;
using Website.MVC.WebApi.Models;
using Website.MVC.WebApi.Security;

namespace Website.MVC.WebApi
{
    [AuthorizationMobile]
    public class TransformadorController : ApiController
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
            IEnumerable<Transformador> TrandormadoresDB = UnitOfWork.TransformadorRepository.Get(trf => trf.IdPoste == IdPoste && trf.DataExclusao == null);
            if (TrandormadoresDB != null)
            {
                return Request.CreateResponse(HttpStatusCode.OK, new ResponseApi() { Status = Status.OK, Results = TrandormadoresDB });
            }
            else
                return Request.CreateResponse(HttpStatusCode.OK, new ResponseApi() { Status = Status.NOK, Message = Resources.Messages.Not_Register_Data_Base });
        }

        [HttpGet]
        [LogExceptionMobile]
        public HttpResponseMessage Get(long IdTransformador)
        {
            Transformador TransformadorDB = UnitOfWork.TransformadorRepository.Get(ip => ip.IdTransformador == IdTransformador && ip.DataExclusao == null).FirstOrDefault();
            if (TransformadorDB != null)
            {
                return Request.CreateResponse(HttpStatusCode.OK, new ResponseApi()
                {
                    Status = Status.OK,
                    Results =
                    new
                    {
                        IdTransformador = TransformadorDB.IdTransformador,
                        IdPoste = TransformadorDB.IdPoste,
                        CodigoGeoBD = TransformadorDB.CodigoGeoBD,
                        Status = TransformadorDB.Status,
                        Proprietario = TransformadorDB.Proprietario,
                        Fase = TransformadorDB.Fase,
                        NumeroCampo = TransformadorDB.NumeroCampo,
                        PotenciaTotal = TransformadorDB.PotenciaTotal,
                        TipoLigacao = TransformadorDB.TipoLigacao,
                        TensaoNominal = TransformadorDB.TensaoNominal,
                        TipoInstalacao = TransformadorDB.TipoInstalacao,
                        CortaCircuito = TransformadorDB.CortaCircuito,
                        Descricao = TransformadorDB.Descricao,
                        NumeroEquipamento = TransformadorDB.NumeroEquipamento
                    }
                });
            }
            else
                return Request.CreateResponse(HttpStatusCode.OK, new ResponseApi() { Status = Status.NOK, Message = Resources.Messages.Not_Register_Data_Base });
        }

        [HttpPut]
        [LogExceptionMobile]
        public HttpResponseMessage Edit(TransformadorEditAPI Transformador)
        {
            /// Validando o Transformador
            ICollection<ValidationResult> results;
            if (Transformador.TryValidate(out results))
            {
                Transformador TransformadorDB = UnitOfWork.TransformadorRepository.Get(trf => trf.IdTransformador == Transformador.IdTransformador && trf.DataExclusao == null, includeProperties: "Poste.Cidade").FirstOrDefault();
                if (TransformadorDB != null)
                {
                    //Atribuindo os novos valores da Edicao
                    TransformadorDB.CodigoGeoBD = Transformador.CodigoGeoBD;
                    TransformadorDB.Status = Transformador.Status;
                    TransformadorDB.Proprietario = Transformador.Proprietario;
                    TransformadorDB.Fase = Transformador.Fase;
                    TransformadorDB.NumeroCampo = Transformador.NumeroCampo;
                    TransformadorDB.PotenciaTotal = Transformador.PotenciaTotal;
                    TransformadorDB.TipoLigacao = Transformador.TipoLigacao;
                    TransformadorDB.TensaoNominal = Transformador.TensaoNominal;
                    TransformadorDB.TipoInstalacao = Transformador.TipoInstalacao;
                    TransformadorDB.CortaCircuito = Transformador.CortaCircuito;
                    TransformadorDB.Descricao = Transformador.Descricao;
                    TransformadorDB.NumeroEquipamento = Transformador.NumeroEquipamento;

                    // Salvando a posição do Mobile no momento da Edição do poste
                    UTM posicaoAtualizacao = new ConverterLatLonToUtm(TransformadorDB.Poste.Cidade.Datum, TransformadorDB.Poste.Cidade.NorteOuSul, TransformadorDB.Poste.Cidade.Zona).Convert(Transformador.LatAtualizacao, Transformador.LonAtualizacao);
                    TransformadorDB.Poste.XAtualizacao = posicaoAtualizacao.X;
                    TransformadorDB.Poste.YAtualizacao = posicaoAtualizacao.Y;

                    UnitOfWork.TransformadorRepository.Update(TransformadorDB);
                    UnitOfWork.Save(); //Commit

                    return Request.CreateResponse(HttpStatusCode.OK, new ResponseApi()
                    {
                        Status = Status.OK,
                        Message = Resources.Messages.Save_OK,
                        Results = new
                        {
                            IdTransformador = TransformadorDB.IdTransformador,
                            IdPoste = TransformadorDB.IdPoste,
                            CodigoGeoBD = TransformadorDB.CodigoGeoBD,
                            Status = TransformadorDB.Status,
                            Proprietario = TransformadorDB.Proprietario,
                            Fase = TransformadorDB.Fase,
                            NumeroCampo = TransformadorDB.NumeroCampo,
                            PotenciaTotal = TransformadorDB.PotenciaTotal,
                            TipoLigacao = TransformadorDB.TipoLigacao,
                            TensaoNominal = TransformadorDB.TensaoNominal,
                            TipoInstalacao = TransformadorDB.TipoInstalacao,
                            CortaCircuito = TransformadorDB.CortaCircuito,
                            Descricao = TransformadorDB.Descricao,
                            NumeroEquipamento = TransformadorDB.NumeroEquipamento
                        }
                    });
                }
                else
                    return Request.CreateResponse(HttpStatusCode.OK, new ResponseApi() { Status = Status.NOK, Message = Resources.Messages.Transformador_Not_Found });
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
        public HttpResponseMessage Add(TransformadorEditAPI Transformador)
        {
            /// Validando o IP
            ICollection<ValidationResult> results;
            if (Transformador.TryValidate(out results))
            {
                Poste PosteBD = UnitOfWork.PosteRepository.Get(os => os.IdPoste == Transformador.IdPoste, includeProperties: "Cidade").FirstOrDefault();
                if (PosteBD != null)
                {
                    // Salvando a posição do Mobile no momento da Edição do poste
                    UTM posicaoAtualizacao = new ConverterLatLonToUtm(PosteBD.Cidade.Datum, PosteBD.Cidade.NorteOuSul, PosteBD.Cidade.Zona).Convert(Transformador.LatAtualizacao, Transformador.LonAtualizacao);
                    PosteBD.XAtualizacao = posicaoAtualizacao.X;
                    PosteBD.YAtualizacao = posicaoAtualizacao.Y;

                    Transformador TransformadorAux = new Transformador()
                    {
                        Poste = PosteBD,
                        CodigoGeoBD = Transformador.CodigoGeoBD,
                        Status = Transformador.Status,
                        Proprietario = Transformador.Proprietario,
                        Fase = Transformador.Fase,
                        NumeroCampo = Transformador.NumeroCampo,
                        PotenciaTotal = Transformador.PotenciaTotal,
                        TipoLigacao = Transformador.TipoLigacao,
                        TensaoNominal = Transformador.TensaoNominal,
                        TipoInstalacao = Transformador.TipoInstalacao,
                        CortaCircuito = Transformador.CortaCircuito,
                        Descricao = Transformador.Descricao,
                        NumeroEquipamento = Transformador.NumeroEquipamento
                    };

                    UnitOfWork.TransformadorRepository.Insert(TransformadorAux);
                    UnitOfWork.Save();

                    return Request.CreateResponse(HttpStatusCode.OK, new ResponseApi()
                    {
                        Status = Status.OK,
                        Message = Resources.Messages.Save_OK,
                        Results = new
                        {
                            IdTransformador = TransformadorAux.IdTransformador,
                            IdPoste = TransformadorAux.IdPoste,
                            CodigoGeoBD = TransformadorAux.CodigoGeoBD,
                            Status = TransformadorAux.Status,
                            Proprietario = TransformadorAux.Proprietario,
                            Fase = TransformadorAux.Fase,
                            NumeroCampo = TransformadorAux.NumeroCampo,
                            PotenciaTotal = TransformadorAux.PotenciaTotal,
                            TipoLigacao = TransformadorAux.TipoLigacao,
                            TensaoNominal = TransformadorAux.TensaoNominal,
                            TipoInstalacao = TransformadorAux.TipoInstalacao,
                            CortaCircuito = TransformadorAux.CortaCircuito,
                            Descricao = TransformadorAux.Descricao,
                            NumeroEquipamento = TransformadorAux.NumeroEquipamento
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