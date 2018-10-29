using System.Web.Http;
using System.Web.Http.Controllers;
using Website.BLL.Utils.Security;
using Website.DAL.UnitOfWork;
using Website.MVC.Helpers.Config;
using Website.MVC.WebApi.Security;
using System.Linq;
using System.Net.Http;
using System.Collections.Generic;
using Website.BLL.Entities;
using System.Net;
using Website.MVC.WebApi.Models;
using Website.BLL.Utils.Geocoding;
using System;
using System.Configuration;
using System.ComponentModel.DataAnnotations;
using Website.MVC.Util;

namespace Website.MVC.WebApi
{
    [AuthorizationMobile]
    public class PosteController : ApiController
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
        public HttpResponseMessage GetByOS(long IdOrdemDeServico)
        {

            IEnumerable<Poste> PostesDB = UnitOfWork.PosteRepository.Get(p => p.IdOrdemDeServico == IdOrdemDeServico && p.DataExclusao == null, includeProperties: "Cidade,Fotos");
            if (PostesDB != null)
            {
                List<object> Postes = new List<object>();
                ConverterUtmToLatLon converter = null;

                foreach (Poste p in PostesDB)
                {
                    if (converter == null)
                        converter = new ConverterUtmToLatLon(p.Cidade.Datum, p.Cidade.NorteOuSul, p.Cidade.Zona);

                    List<FotoAPI> FotosApi = new List<FotoAPI>();

                    foreach (FotoPoste foto in p.Fotos.Where(f => f.DataExclusao == null))
                    {
                        FotoAPI ft = new FotoAPI();
                        ft.NumeroFoto = foto.NumeroFoto;
                        ft.DataFoto = foto.DataFoto.ToString("dd-MM-yyyy HH:MM:SS"); //ConvertDate.DateTimeToUnixTimestamp(foto.DataFoto);
                        FotosApi.Add(ft);
                    }

                    Postes.Add(new
                    {
                        IdPoste = p.IdPoste,
                        Posicao = converter.Convert(p.X, p.Y),
                        DataCadastro = p.DataCadastro,
                        DataExclusao = p.DataExclusao,
                        Finalizado = p.Finalizado,
                        CodigoGeo = p.CodigoGeo,
                        IdLogradouro = p.IdLogradouro,
                        IdCidade = p.Cidade.IdCidade,
                        IdOrdemDeServico = p.IdOrdemDeServico,
                        Fotos = FotosApi,
                        Altura = p.Altura,
                        TipoPoste = p.TipoPoste,
                        Esforco = p.Esforco,
                        Descricao = p.Descricao
                    });
                }

                return Request.CreateResponse(HttpStatusCode.OK, new ResponseApi() { Status = Status.OK, Results = Postes });
            }
            else
                return Request.CreateResponse(HttpStatusCode.OK, new ResponseApi() { Status = Status.NOK, Message = Resources.Messages.Not_Register_Data_Base });
        }

        [HttpGet]
        [LogExceptionMobile]
        public HttpResponseMessage Get(long IdPoste)
        {
            Poste PostesDB = UnitOfWork.PosteRepository.Get(p => p.IdPoste == IdPoste && p.DataExclusao == null, includeProperties: "Cidade,Fotos").FirstOrDefault();
            if (PostesDB != null)
            {
                object Poste = new object();
                ConverterUtmToLatLon converter = null;

                if (converter == null)
                    converter = new ConverterUtmToLatLon(PostesDB.Cidade.Datum, PostesDB.Cidade.NorteOuSul, PostesDB.Cidade.Zona);

                List<FotoAPI> FotosApi = new List<FotoAPI>();

                foreach (FotoPoste foto in PostesDB.Fotos.Where(f => f.DataExclusao == null))
                {
                    FotoAPI ft = new FotoAPI();
                    ft.NumeroFoto = foto.NumeroFoto;
                    ft.DataFoto = foto.DataFoto.ToString("dd-MM-yyyy hh:mm:ss"); //ConvertDate.DateTimeToUnixTimestamp(foto.DataFoto);
                    FotosApi.Add(ft);
                }
       
                Poste = new
                {
                    IdPoste = PostesDB.IdPoste,
                    Posicao = converter.Convert(PostesDB.X, PostesDB.Y),
                    DataCadastro = PostesDB.DataCadastro,
                    DataExclusao = PostesDB.DataExclusao,
                    Finalizado = PostesDB.Finalizado,
                    CodigoGeo = PostesDB.CodigoGeo,
                    IdLogradouro = PostesDB.IdLogradouro,
                    IdCidade = PostesDB.Cidade.IdCidade,
                    IdOrdemDeServico = PostesDB.IdOrdemDeServico,
                    Fotos = FotosApi,
                    Altura = PostesDB.Altura,
                    TipoPoste = PostesDB.TipoPoste,
                    Esforco = PostesDB.Esforco,
                    Descricao = PostesDB.Descricao
                };

                return Request.CreateResponse(HttpStatusCode.OK, new ResponseApi() { Status = Status.OK, Results = Poste });
            }
            else
                return Request.CreateResponse(HttpStatusCode.OK, new ResponseApi() { Status = Status.NOK, Message = Resources.Messages.Not_Register_Data_Base });
        }

        [HttpPut]
        [LogExceptionMobile]
        public HttpResponseMessage Edit(PosteEditAPI Poste)
        {
            /// Validando o Poste
            ICollection<ValidationResult> results;
            if (Poste.TryValidate(out results))
            {
                Poste posteBD = UnitOfWork.PosteRepository.Get(p => p.IdPoste == Poste.IdPoste && p.DataExclusao == null, includeProperties: "Cidade,Fotos,OrdemDeServico").FirstOrDefault();
                if (posteBD != null)
                {
                    ConverterLatLonToUtm converter = new ConverterLatLonToUtm(posteBD.Cidade.Datum, posteBD.Cidade.NorteOuSul, posteBD.Cidade.Zona);
                    UTM utmPoste = converter.Convert(Poste.Lat, Poste.Lon);

                    //Atribuindo os novos valores da Edicao
                    posteBD.X = utmPoste.X;
                    posteBD.Y = utmPoste.Y;
                    posteBD.Finalizado = Poste.Finalizado;
                    posteBD.Altura = Poste.Altura;
                    posteBD.TipoPoste = Poste.TipoPoste;
                    posteBD.Esforco = Poste.Esforco != null ? Poste.Esforco : 0;
                    posteBD.Descricao = Poste.Descricao != null ? Poste.Descricao.ToUpper() : "";
                    posteBD.DataCadastro = ConvertDate.UnixTimestampToDateTime(Poste.DataAtualizacao);

                    // Salvando a posição do Mobile no momento da Edição do poste
                    UTM posicaoAtualizacao = converter.Convert(Poste.LatAtualizacao, Poste.LonAtualizacao);
                    posteBD.XAtualizacao = posicaoAtualizacao.X;
                    posteBD.YAtualizacao = posicaoAtualizacao.Y;

                    UnitOfWork.PosteRepository.Update(posteBD);

                    /// Setando DataDeExclusao em todas as fotos                    
                    foreach (FotoPoste f in posteBD.Fotos)
                    {
                        f.DataExclusao = DateTime.Now;
                        UnitOfWork.FotoPosteRepository.Update(f);
                    }

                    /// usuario
                    Usuario User = UnitOfWork.UsuarioRepository.Get(u => u.IdUsuario == IdUsuario).FirstOrDefault();

                    // Verificando as alterações das fotos
                    if (Poste.Fotos != null && Poste.Fotos.Count > 0)
                    {
                        foreach (FotoAPI foto in Poste.Fotos)
                        {
                            if (foto != null)
                            {
                                DateTime DataDiretorio = Convert.ToDateTime(foto.DataFoto);  //ConvertDate.UnixTimestampToDateTime(foto.DataFoto);
                                String Data = DataDiretorio.ToString("dd-MM-yyyy hh:mm:ss");

                                FotoPoste f = UnitOfWork.FotoPosteRepository.Get(fto => fto.IdPoste == posteBD.IdPoste && fto.NumeroFoto.Trim() == foto.NumeroFoto.Trim()).FirstOrDefault();
                                if (f != null)
                                {
                                    f.DataExclusao = null;
                                    f.DataFoto = DataDiretorio;
                                    f.Path = string.Format(ConfigurationManager.AppSettings["NewPathFotos"], posteBD.Cidade.CidadeDiretorio, Data, User.UserName.ToUpper(), foto.NumeroFoto.Trim());
                                    UnitOfWork.FotoPosteRepository.Update(f);
                                }
                                else
                                {
                                    FotoPoste fAux = new FotoPoste
                                    {
                                        CodigoGeoBD = -1,
                                        IdPoste = posteBD.IdPoste,
                                        NumeroFoto = foto.NumeroFoto.Trim(),
                                        DataFoto = DataDiretorio,
                                        Path = string.Format(ConfigurationManager.AppSettings["NewPathFotos"], posteBD.Cidade.CidadeDiretorio, Data, User.UserName.ToUpper(), foto.NumeroFoto.Trim())
                                    };

                                    UnitOfWork.FotoPosteRepository.Insert(fAux);
                                }
                            }                            
                        }
                    }

                    UnitOfWork.Save(); //Commit

                    List<FotoAPI> FotosApi = new List<FotoAPI>();

                    foreach (FotoPoste foto in posteBD.Fotos.Where(f => f.DataExclusao == null))
                    {
                        FotoAPI ft = new FotoAPI();
                        ft.NumeroFoto = foto.NumeroFoto;
                        ft.DataFoto = foto.DataFoto.ToString("dd-MM-yyyy hh:mm:ss"); //ConvertDate.DateTimeToUnixTimestamp(foto.DataFoto);
                        FotosApi.Add(ft);
                    }

                    ConverterUtmToLatLon converterToLatLong = new ConverterUtmToLatLon(posteBD.Cidade.Datum, posteBD.Cidade.NorteOuSul, posteBD.Cidade.Zona);
                    return Request.CreateResponse(HttpStatusCode.OK, new ResponseApi()
                    {
                        Status = Status.OK,
                        Message = Resources.Messages.Save_OK,
                        Results = new
                        {
                            IdPoste = posteBD.IdPoste,
                            Posicao = converterToLatLong.Convert(posteBD.X, posteBD.Y),
                            DataCadastro = posteBD.DataCadastro,
                            DataExclusao = posteBD.DataExclusao,
                            Finalizado = posteBD.Finalizado,
                            CodigoGeo = posteBD.CodigoGeo,
                            IdLogradouro = posteBD.IdLogradouro,
                            IdCidade = posteBD.Cidade.IdCidade,
                            IdOrdemDeServico = posteBD.IdOrdemDeServico,
                            Fotos = FotosApi,
                            Altura = posteBD.Altura,
                            TipoPoste = posteBD.TipoPoste,
                            Esforco = posteBD.Esforco,
                            Descricao = posteBD.Descricao
                        }
                    });
                }
                else
                    return Request.CreateResponse(HttpStatusCode.OK, new ResponseApi() { Status = Status.NOK, Message = Resources.Messages.Poste_Not_Found });
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
        public HttpResponseMessage Add(PosteEditAPI Poste)
        {
            /// Validando o Poste
            ICollection<ValidationResult> results;
            if (Poste.TryValidate(out results))
            {
                OrdemDeServico OSBD = UnitOfWork.OrdemDeServicoRepository.Get(os => os.IdOrdemDeServico == Poste.IdOrdemDeServico, includeProperties: "Cidade").FirstOrDefault();
                if (OSBD != null)
                {
                    ConverterLatLonToUtm converter = new ConverterLatLonToUtm(OSBD.Cidade.Datum, OSBD.Cidade.NorteOuSul, OSBD.Cidade.Zona);
                    UTM utmPoste = converter.Convert(Poste.Lat, Poste.Lon);
                    
                    // Salvando a posição do Mobile no momento da Edição do poste
                    UTM posicaoAtualizacao = converter.Convert(Poste.LatAtualizacao, Poste.LonAtualizacao);

                    Poste posteAux = new Poste()
                    {
                        X = utmPoste.X,
                        Y = utmPoste.Y,
                        XAtualizacao = posicaoAtualizacao.X,
                        YAtualizacao = posicaoAtualizacao.Y,
                        OrdemDeServico = OSBD,
                        DataCadastro = DateTime.Now,
                        Cidade = OSBD.Cidade,
                        Altura = Poste.Altura,
                        TipoPoste = Poste.TipoPoste,
                        Esforco = Poste.Esforco,
                        Descricao = Poste.Descricao
                    };

                    UnitOfWork.PosteRepository.Insert(posteAux);

                    /// usuario
                    Usuario User = UnitOfWork.UsuarioRepository.Get(u => u.IdUsuario == IdUsuario).FirstOrDefault();

                    if (Poste.Fotos != null && Poste.Fotos.Count > 0)
                    {
                        foreach (FotoAPI foto in Poste.Fotos)
                        {
                            if (foto != null)
                            {
                                DateTime DataDiretorio = Convert.ToDateTime(foto.DataFoto); //ConvertDate.UnixTimestampToDateTime(foto.DataFoto);
                                String Data = DataDiretorio.ToString("dd-MM-yyyy hh:mm:ss");

                                UnitOfWork.FotoPosteRepository.Insert(new FotoPoste
                                {
                                    CodigoGeoBD = -1,
                                    IdPoste = posteAux.IdPoste,
                                    NumeroFoto = foto.NumeroFoto.Trim(),
                                    DataFoto = DataDiretorio,
                                    Path = string.Format(ConfigurationManager.AppSettings["NewPathFotos"], OSBD.Cidade.CidadeDiretorio, Data, User.UserName.ToUpper(), foto.NumeroFoto.Trim())                                
                                });
                            }
                        }
                    }

                    UnitOfWork.Save();

                    List<FotoAPI> FotosApi = new List<FotoAPI>();

                    foreach (FotoPoste foto in posteAux.Fotos.Where(f => f.DataExclusao == null))
                    {
                        FotoAPI ft = new FotoAPI();
                        ft.NumeroFoto = foto.NumeroFoto;
                        ft.DataFoto = foto.DataFoto.ToString("dd-MM-yyyy hh:mm:ss"); //ConvertDate.DateTimeToUnixTimestamp(foto.DataFoto);
                        FotosApi.Add(ft);
                    }

                    ConverterUtmToLatLon converterToLatLong = new ConverterUtmToLatLon(posteAux.Cidade.Datum, posteAux.Cidade.NorteOuSul, posteAux.Cidade.Zona);
                    return Request.CreateResponse(HttpStatusCode.OK, new ResponseApi()
                    {
                        Status = Status.OK,
                        Message = Resources.Messages.Save_OK,
                        Results = new
                        {
                            IdPoste = posteAux.IdPoste,
                            Posicao = converterToLatLong.Convert(posteAux.X, posteAux.Y),
                            DataCadastro = posteAux.DataCadastro,
                            DataExclusao = posteAux.DataExclusao,
                            Finalizado = posteAux.Finalizado,
                            CodigoGeo = posteAux.CodigoGeo,
                            IdLogradouro = posteAux.IdLogradouro,
                            IdCidade = posteAux.Cidade.IdCidade,
                            IdOrdemDeServico = posteAux.IdOrdemDeServico,
                            Fotos = FotosApi,
                            Altura = posteAux.Altura,
                            TipoPoste = posteAux.TipoPoste,
                            Esforco = posteAux.Esforco,
                            Descricao = posteAux.Descricao
                        }
                    });
                }
                else
                    return Request.CreateResponse(HttpStatusCode.OK, new ResponseApi() { Status = Status.NOK, Message = Resources.Messages.OS_Not_Found });
            }
            else
            {
                string MessageValidate = string.Empty;
                foreach (var validationResult in results) MessageValidate += validationResult.ErrorMessage;
                return Request.CreateResponse(HttpStatusCode.OK, new ResponseApi() { Status = Status.NOK, Message = MessageValidate });
            }
        }

        [HttpPost]
        [LogExceptionMobile]
        public HttpResponseMessage Deletar(PosteEditAPI poste)
        {
            Poste PostesDB = UnitOfWork.PosteRepository.Get(p => p.IdPoste == poste.IdPoste && p.DataExclusao == null, includeProperties: "Cidade").FirstOrDefault();
            if (PostesDB != null)
            {
                // Salvando a posição do Mobile no momento da exclusao do poste
                UTM posicaoAtualizacao = new ConverterLatLonToUtm(PostesDB.Cidade.Datum, PostesDB.Cidade.NorteOuSul, PostesDB.Cidade.Zona).Convert(poste.LatAtualizacao, poste.LonAtualizacao);
                PostesDB.XAtualizacao = posicaoAtualizacao.X;
                PostesDB.YAtualizacao = posicaoAtualizacao.Y;
                UnitOfWork.PosteRepository.Update(PostesDB);

                UnitOfWork.PosteRepository.SetDataExclusao(PostesDB);
                UnitOfWork.Save();

                return Request.CreateResponse(HttpStatusCode.OK, new ResponseApi() { Status = Status.OK, Message = Resources.Messages.Save_OK });
            }
            else
                return Request.CreateResponse(HttpStatusCode.OK, new ResponseApi() { Status = Status.NOK, Message = Resources.Messages.Not_Register_Data_Base });
        }

        /// <summary>
        /// Formatar as fotos para String
        /// </summary>
        /// <param name="fotos"></param>
        /// <returns></returns>
        private string FormatFotos(ICollection<FotoPoste> fotos)
        {
            return string.Join(" / ", fotos.Select(f => f.NumeroFoto));
        }
    }
}