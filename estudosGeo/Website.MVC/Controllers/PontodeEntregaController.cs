using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Website.BLL.Entities;
using Website.BLL.Utils.Geocoding;
using Website.DAL.UnitOfWork;
using Website.Identity.CustomAutorizes;
using Website.MVC.Helpers.CustomAttribute;
using Website.MVC.Helpers.CustomController;
using Website.MVC.Models.Maps;

namespace Website.MVC.Controllers
{
    public class PontodeEntregaController : BaseController
    {
        private readonly UnitOfWork UnitOfWork = new UnitOfWork();

        /// <summary>
        /// Método que altera o tamanho max do retorno do Json.
        /// </summary>
        /// <param name="data"></param>
        /// <param name="contentType"></param>
        /// <param name="contentEncoding"></param>
        /// <param name="behavior"></param>
        /// <returns></returns>
        protected JsonResult SendBigJson(object data, JsonRequestBehavior behavior)
        {
            return new JsonResult()
            {
                Data = data,
                JsonRequestBehavior = behavior,
                MaxJsonLength = int.MaxValue // Setando o tamanho max para o retorno do Json
            };
        }

        [HttpGet]
        [AppAuthorize(Modulos = new Modules[] { Modules.REDE }, Permissoes = new Permissions[] { Permissions.CONSULTAR })]
        public ActionResult GetPontoEntregaByOs(string codOs)
        {
            IEnumerable<PontoEntrega> ponto_entregas_bd = UnitOfWork.PontoEntregaRepository.Get(
                p => p.OrdemDeServico.NumeroOS == codOs &&
                p.DataExclusao == null, includeProperties: "OrdemDeServico,Cidade");

            if (ponto_entregas_bd != null && ponto_entregas_bd.Count() > 0)
                return Json(new
                {
                    PontoEntregas = PontoPaginadoView.GenerateByOs(ponto_entregas_bd)
                    //Limites = new LimitesCidadeView().LimitesByOS(UnitOfWork.OrdemDeServicoRepository.Get(o => o.NumeroOS == codOs, includeProperties: "Cidade,PoligonosOS").FirstOrDefault())
                }, JsonRequestBehavior.AllowGet);
            else
                return Json(new { Msg = Resources.Messages.Not_Register_Data_Base }, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        [AppAuthorize(Modulos = new Modules[] { Modules.REDE }, Permissoes = new Permissions[] { Permissions.CONSULTAR })]
        public ActionResult GetListaPontodeEntrega(long IdPoste)
        {
            //Lista de Ponto de entrega do Banco de Dados.
            //Tranforma a lista de pontos de entregas retornados do banco para um objeto que javascript entende.
            //envia a lista de pontos de entrega para a javascript
            return Json(new MetodosPontoEntregaView().ListaPontoEntrega(UnitOfWork.PontoEntregaRepository.Get(p => p.IdPoste == IdPoste, includeProperties: "Medidor").ToList(), IdPoste), JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        [AppAuthorize(Modulos = new Modules[] { Modules.REDE }, Permissoes = new Permissions[] { Permissions.CONSULTAR })]
        public ActionResult GetPontodeEntrega(long IdPontoEntrega)
        {
            return Json(new MetodosPontoEntregaView().PontoEntregaToPontoEntregaView(UnitOfWork.PontoEntregaRepository.Get(p => p.IdPontoEntrega == IdPontoEntrega).FirstOrDefault()), JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        [AppAuthorize(Modulos = new Modules[] { Modules.REDE }, Permissoes = new Permissions[] { Permissions.CONSULTAR })]
        public ActionResult GetMedidores(long IdPontoEntrega)
        {
            return Json(new MetodosPontoEntregaView().MedidoresPontoEntrega(UnitOfWork.MedidoresRepository.Get(m => m.IdPontoEntrega == IdPontoEntrega).ToList()), JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        [AppAuthorize(Modulos = new Modules[] { Modules.REDE }, Permissoes = new Permissions[] { Permissions.ATUALIZAR })]
        public ActionResult SalvarPontoEntrega(PontoEntregaView PontoEntregaReceived)
        {

            if (PontoEntregaReceived != null)
            {
                //Poste poste = UnitOfWork.PosteRepository.Get(p => p.IdPoste == PontoEntregaReceived.IdPoste).FirstOrDefault();
                Cidade cidade = UnitOfWork.CidadeRepository.Get(c => c.IdCidade == PontoEntregaReceived.IdCidade).FirstOrDefault();

                ConverterLatLonToUtm converter = new ConverterLatLonToUtm(cidade.Datum, cidade.NorteOuSul, cidade.Zona);
                UTM utm = converter.Convert(PontoEntregaReceived.Latitude, PontoEntregaReceived.Longitude);

                if (PontoEntregaReceived.IdPontoEntrega != -1)
                {
                    #region Atualiza Ponto de Entrega

                    PontoEntrega ponto_entrega_bd = UnitOfWork.PontoEntregaRepository.Get(p => p.IdPontoEntrega == PontoEntregaReceived.IdPontoEntrega).FirstOrDefault();

                    // ponto_entrega_bd.IdPontoEntrega = PontoEntregaReceived.IdPontoEntrega;
                    //ponto_entrega_bd.IdPoste = PontoEntregaReceived.IdPoste;
                    ponto_entrega_bd.CodigoGeoBD = PontoEntregaReceived.CodigoGeoBD;
                    //   ponto_entrega_bd.IdOrdemDeServico = PontoEntregaReceived.IdOrdemServico;

                    ponto_entrega_bd.Classificacao = PontoEntregaReceived.Classificacao;
                    ponto_entrega_bd.Complemento1 = PontoEntregaReceived.Complemento1;
                    //ponto_entrega_bd.Complemento2 = PontoEntregaReceived.Complemento2;                  

                    //ponto_entrega_bd.Status = PontoEntregaReceived.Status;
                    // ponto_entrega_bd.ClasseAtendimento = PontoEntregaReceived.ClasseAtendimento;
                    //ponto_entrega_bd.TipoConstrucao = PontoEntregaReceived.TipoConstrucao;
                    ponto_entrega_bd.Numero = PontoEntregaReceived.Numero;
                    ponto_entrega_bd.ClasseSocial = PontoEntregaReceived.ClasseSocial;
                    ponto_entrega_bd.IdLagradouro = PontoEntregaReceived.IdLogradouro;

                    ponto_entrega_bd.Classificacao = PontoEntregaReceived.Classificacao;
                    ponto_entrega_bd.Ocorrencia = PontoEntregaReceived.Ocorrencia;
                    ponto_entrega_bd.QtdDomicilio = PontoEntregaReceived.QtdDomicilio;
                    ponto_entrega_bd.NumeroAndaresEdificio = PontoEntregaReceived.NumeroAndaresEdificio;
                    ponto_entrega_bd.TotalApartamentosEdificio = PontoEntregaReceived.TotalApartamentosEdificio;
                    ponto_entrega_bd.NomeEdificio = PontoEntregaReceived.NomeEdificio;
                    ponto_entrega_bd.QtdBlocos = PontoEntregaReceived.QtdBlocos;
                    ponto_entrega_bd.TipoComercio = PontoEntregaReceived.ClassificacaoComercio;
                    ponto_entrega_bd.QtdSalas = PontoEntregaReceived.QtdDomicilioComercio;                    
                    //ponto_entrega_bd.Fase = PontoEntregaReceived.Fase;
                    //ponto_entrega_bd.EtLigacao = PontoEntregaReceived.EtLigacao;
                    // ponto_entrega_bd.Observacao = PontoEntregaReceived.Observacao;
                    //ponto_entrega_bd.X = utm.X;
                    // ponto_entrega_bd.Y = utm.Y;

                    /// Atualizando o Ponto Entrega
                    UnitOfWork.PontoEntregaRepository.Update(ponto_entrega_bd);

                    List<Medidor> medidores_ponto_entrega = UnitOfWork.MedidoresRepository.Get(m => m.IdPontoEntrega == PontoEntregaReceived.IdPontoEntrega).ToList();

                    /// Apagando os Medidores do Ponto de Entrega
                    foreach (Medidor medidor_bd in medidores_ponto_entrega)
                    {
                        UnitOfWork.MedidoresRepository.Delete(medidor_bd);
                    }

                    /* if (PontoEntregaReceived.Medidores != null)
                     {
                         foreach (MedidorView medidor_view in PontoEntregaReceived.Medidores)
                         {
                             /// Inserindo os Medidores
                             UnitOfWork.MedidoresRepository.Insert(new Medidor
                             {
                                 IdPontoEntrega = medidor_view.IdPontoEntrega,
                                 NumeroMedidor = medidor_view.NumeroMedidor,
                                 ComplementoResidencial = medidor_view.ComplementoResidencial
                             });
                         }
                     }*/

                    /// Apagando Fotos do ponto de Entrega
                    List<FotoPontoEntrega> fotos_BD = UnitOfWork.FotoPontoEntregaRepository.Get(p => p.IdPontoEntrega == PontoEntregaReceived.IdPontoEntrega).ToList();

                    foreach (FotoPontoEntrega foto_bd in fotos_BD)
                    {
                        UnitOfWork.FotoPontoEntregaRepository.Delete(foto_bd);
                    }

                    if (PontoEntregaReceived.Fotos != null)
                    {
                        foreach (FotoPontoEntregaView foto in PontoEntregaReceived.Fotos)
                        {
                            /// Nao deve Vim foto_view.DataFoto vazio mas se vim nao quebra o codigo
                            DateTime DataDiretorio = foto.DataFoto != String.Empty ? Convert.ToDateTime(foto.DataFoto) : DateTime.Now;
                            String Data = DataDiretorio.ToString("ddMMyyyy");

                            UnitOfWork.FotoPontoEntregaRepository.Insert(
                                new FotoPontoEntrega()
                                {
                                    CodigoGeoBD = -1,
                                    DataFoto = Convert.ToDateTime(foto.DataFoto),
                                    IdPontoEntrega = PontoEntregaReceived.IdPontoEntrega,
                                    NumeroFoto = foto.NumeroFoto.Trim(),
                                    DataExclusao = null,
                                    Path = string.Format(ConfigurationManager.AppSettings["NewPathFotos"], cidade.CidadeDiretorio, Data, User.Identity.Name.ToUpper(), foto.NumeroFoto.Trim().ToUpper())
                                });
                        }
                    }


                    //Salvando as Alteraçoes
                    UnitOfWork.Save();


                    #endregion
                }
                else
                {
                    #region Novo Ponto de Entrega

                    PontoEntrega new_ponto_entrega = new PontoEntrega();

                    new_ponto_entrega.IdPoste = PontoEntregaReceived.IdPoste;
                    new_ponto_entrega.CodigoGeoBD = PontoEntregaReceived.CodigoGeoBD;
                    // new_ponto_entrega.Status = PontoEntregaReceived.Status;
                    // new_ponto_entrega.ClasseAtendimento = PontoEntregaReceived.ClasseAtendimento;
                    // new_ponto_entrega.TipoConstrucao = PontoEntregaReceived.TipoConstrucao;
                    new_ponto_entrega.Numero = PontoEntregaReceived.Numero;
                    new_ponto_entrega.ClasseSocial = PontoEntregaReceived.ClasseSocial;
                    // new_ponto_entrega.Logradouro = PontoEntregaReceived.Logradouro;
                    // new_ponto_entrega.Fase = PontoEntregaReceived.Fase;
                    // new_ponto_entrega.EtLigacao = PontoEntregaReceived.EtLigacao;
                    // new_ponto_entrega.Observacao = PontoEntregaReceived.Observacao;
                    new_ponto_entrega.X = utm.X;
                    new_ponto_entrega.Y = utm.Y;

                    /// Atualizando o Ponto Entrega
                    UnitOfWork.PontoEntregaRepository.Insert(new_ponto_entrega);

                    /*  if (PontoEntregaReceived.Medidores != null)
                      {
                          foreach (MedidorView medidor_view in PontoEntregaReceived.Medidores)
                          {
                              /// Inserindo os Medidores
                              UnitOfWork.MedidoresRepository.Insert(new Medidor
                              {
                                  IdPontoEntrega = new_ponto_entrega.IdPontoEntrega,
                                  NumeroMedidor = medidor_view.NumeroMedidor,
                                  ComplementoResidencial = medidor_view.ComplementoResidencial
                              });
                          }
                      }*/

                    foreach (FotoPontoEntregaView foto in PontoEntregaReceived.Fotos)
                    {
                        /// Nao deve Vim foto_view.DataFoto vazio mas se vim nao quebra o codigo
                        DateTime DataDiretorio = foto.DataFoto != String.Empty ? Convert.ToDateTime(foto.DataFoto) : DateTime.Now;
                        String Data = DataDiretorio.ToString("ddMMyyyy");

                        FotoPontoEntrega novaFoto = new FotoPontoEntrega();
                        novaFoto.CodigoGeoBD = -1;
                        novaFoto.DataFoto = Convert.ToDateTime(foto.DataFoto);
                        novaFoto.IdPontoEntrega = new_ponto_entrega.IdPontoEntrega;
                        novaFoto.NumeroFoto = foto.NumeroFoto.Trim();
                        novaFoto.DataExclusao = null;
                        novaFoto.Path = string.Format(ConfigurationManager.AppSettings["NewPathFotos"], cidade.CidadeDiretorio, Data, User.Identity.Name.ToUpper(), foto.NumeroFoto.Trim().ToUpper());
                        novaFoto.PontoEntrega = new_ponto_entrega;

                        UnitOfWork.FotoPontoEntregaRepository.Insert(novaFoto);
                    }

                    //Salvando as Alteraçoes
                    UnitOfWork.Save();

                    #endregion
                }

                return Json(new ResponseView() { Status = Status.OK, Result = Resources.Messages.Save_OK }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(new ResponseView() { Status = Status.NOK, Result = Resources.Messages.Error_Save_Changes }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        [AppAuthorize(Modulos = new Modules[] { Modules.REDE }, Permissoes = new Permissions[] { Permissions.ATUALIZAR })]
        public ActionResult SalvarPontoEntregaNovo(PontoEntregaView PontoEntregaReceived)
        {

            Cidade Cidade = UnitOfWork.CidadeRepository.Get(c => c.IdCidade == PontoEntregaReceived.IdCidade).FirstOrDefault();

            if (Cidade != null)
            {

                double latPonto = Double.Parse(PontoEntregaReceived.LatitudeTexto, System.Globalization.CultureInfo.InvariantCulture);
                double lonPonto = Double.Parse(PontoEntregaReceived.LongitudeTexto, System.Globalization.CultureInfo.InvariantCulture);

                //double latPoste = Double.Parse(PontoEntregaReceived.LatitudePosteTexto, System.Globalization.CultureInfo.InvariantCulture);
              //  double lonPoste = Double.Parse(PontoEntregaReceived.LongitudePosteTexto, System.Globalization.CultureInfo.InvariantCulture);

                OrdemDeServico ordemServico = UnitOfWork.OrdemDeServicoRepository.Get(or => or.NumeroOS == PontoEntregaReceived.IdOrdemServicoTexto).FirstOrDefault();

                ConverterLatLonToUtm converter = new ConverterLatLonToUtm(Cidade.Datum, Cidade.NorteOuSul, Cidade.Zona);
                UTM utmPonto = converter.Convert(latPonto, lonPonto);
            //    UTM utmPoste = converter.Convert(latPoste, lonPoste);

                PontoEntrega new_ponto_entrega = new PontoEntrega();

                new_ponto_entrega.IdCidade = PontoEntregaReceived.IdCidade;
                new_ponto_entrega.IdPoste = PontoEntregaReceived.IdPoste;
                new_ponto_entrega.CodigoGeoBD = PontoEntregaReceived.CodigoGeoBD;
                new_ponto_entrega.Complemento1 = PontoEntregaReceived.Complemento1;
                // new_ponto_entrega.ClasseAtendimento = PontoEntregaReceived.ClasseAtendimento;
                // new_ponto_entrega.TipoConstrucao = PontoEntregaReceived.TipoConstrucao;
                new_ponto_entrega.Numero = PontoEntregaReceived.Numero;
                new_ponto_entrega.ClasseSocial = PontoEntregaReceived.ClasseSocial;
                // new_ponto_entrega.Logradouro = PontoEntregaReceived.Logradouro;
                // new_ponto_entrega.Fase = PontoEntregaReceived.Fase;
                // new_ponto_entrega.EtLigacao = PontoEntregaReceived.EtLigacao;
                // new_ponto_entrega.Observacao = PontoEntregaReceived.Observacao;
                new_ponto_entrega.X = utmPonto.X;
                new_ponto_entrega.Y = utmPonto.Y;
                new_ponto_entrega.IdOrdemDeServico = ordemServico.IdOrdemDeServico;
                new_ponto_entrega.DataInclusao = DateTime.Now;

                new_ponto_entrega.Classificacao = PontoEntregaReceived.Classificacao;
                new_ponto_entrega.Ocorrencia = PontoEntregaReceived.Ocorrencia;
                new_ponto_entrega.QtdDomicilio = PontoEntregaReceived.QtdDomicilio;
                new_ponto_entrega.QtdSalas = PontoEntregaReceived.QtdDomicilioComercio;
                new_ponto_entrega.NumeroAndaresEdificio = PontoEntregaReceived.NumeroAndaresEdificio;
                new_ponto_entrega.TotalApartamentosEdificio = PontoEntregaReceived.TotalApartamentosEdificio;
                new_ponto_entrega.NomeEdificio = PontoEntregaReceived.NomeEdificio;
                new_ponto_entrega.QtdBlocos = PontoEntregaReceived.QtdBlocos;
                new_ponto_entrega.TipoComercio = PontoEntregaReceived.ClassificacaoComercio;


                /// Atualizando o Ponto Entrega
                UnitOfWork.PontoEntregaRepository.Insert(new_ponto_entrega);

                /*  if (PontoEntregaReceived.Medidores != null)
                  {
                      foreach (MedidorView medidor_view in PontoEntregaReceived.Medidores)
                      {
                          /// Inserindo os Medidores
                          UnitOfWork.MedidoresRepository.Insert(new Medidor
                          {
                              IdPontoEntrega = new_ponto_entrega.IdPontoEntrega,
                              NumeroMedidor = medidor_view.NumeroMedidor,
                              ComplementoResidencial = medidor_view.ComplementoResidencial
                          });
                      }
                  }*/

                /*   foreach (FotoPontoEntregaView foto in PontoEntregaReceived.Fotos)
                   {
                       /// Nao deve Vim foto_view.DataFoto vazio mas se vim nao quebra o codigo
                       DateTime DataDiretorio = foto.DataFoto != String.Empty ? Convert.ToDateTime(foto.DataFoto) : DateTime.Now;
                       String Data = DataDiretorio.ToString("ddMMyyyy");

                       FotoPontoEntrega novaFoto = new FotoPontoEntrega();
                       novaFoto.CodigoGeoBD = -1;
                       novaFoto.DataFoto = Convert.ToDateTime(foto.DataFoto);
                       novaFoto.IdPontoEntrega = new_ponto_entrega.IdPontoEntrega;
                       novaFoto.NumeroFoto = foto.NumeroFoto.Trim();
                       novaFoto.DataExclusao = null;
                       novaFoto.Path = string.Format(ConfigurationManager.AppSettings["NewPathFotos"], Cidade.CidadeDiretorio, Data, User.Identity.Name.ToUpper(), foto.NumeroFoto.Trim().ToUpper());
                       novaFoto.PontoEntrega = new_ponto_entrega;

                       UnitOfWork.FotoPontoEntregaRepository.Insert(novaFoto);
                   }*/
                UnitOfWork.Save();
                PontoEntrega pontoEntrega = UnitOfWork.PontoEntregaRepository.Get(p => p.IdPontoEntrega == new_ponto_entrega.IdPontoEntrega).FirstOrDefault();
                
                Poste Poste = UnitOfWork.PosteRepository.Get(p => p.IdPoste == PontoEntregaReceived.IdPoste).FirstOrDefault();                
                if (Poste != null)
                {
                    VaosDemandaPoste vaosDemandaPoste = new VaosDemandaPoste
                    {
                        IdCidade = PontoEntregaReceived.IdCidade,
                        IdOrdemDeServico = ordemServico.IdOrdemDeServico,
                        IdPoste = PontoEntregaReceived.IdPoste,
                        IdPontoEntrega = new_ponto_entrega.IdPontoEntrega,
                        X1 = Poste.X,
                        Y1 = Poste.Y,
                        X2 = utmPonto.X,
                        Y2 = utmPonto.Y,
                    };
                    UnitOfWork.VaosDemandaPosteRepository.Insert(vaosDemandaPoste);
                    UnitOfWork.Save();

                    VaosDemandaPoste VaosDemandaPoste = UnitOfWork.VaosDemandaPosteRepository.Get(v => v.IdVaosDemandaPoste == vaosDemandaPoste.IdVaosDemandaPoste).FirstOrDefault();                    

                    //                return Json(new MetodosPontoEntregaView().PontoEntregaToPontoEntregaView(PontoEntrega), JsonRequestBehavior.AllowGet);

                    return Json(new
                    {
                        Demanda = new MetodosPontoEntregaView().PontoEntregaToPontoEntregaView(pontoEntrega),
                        VaoDemandaPoste = new VaosDemandasPaginadoView().VaoToVaoView(VaosDemandaPoste)
                    }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(new
                    {
                        Demanda = new MetodosPontoEntregaView().PontoEntregaToPontoEntregaView(pontoEntrega)
                    },
                    JsonRequestBehavior.AllowGet);
                }

            }
            else
            {
                return Json(new ResponseView() { Status = Status.NotFound, Result = "Cidade nao encontrada" }, JsonRequestBehavior.AllowGet);
            }


        }

        [HttpGet]
        [AppAuthorize(Modulos = new Modules[] { Modules.REDE }, Permissoes = new Permissions[] { Permissions.CONSULTAR })]
        public ActionResult PontoEntregaCoordenada(long IdCidade)
        {
            return Json(new MetodosPontoEntregaView().CoordenadasPontoEntrega(IdCidade), JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        [AppAuthorize(Modulos = new Modules[] { Modules.REDE }, Permissoes = new Permissions[] { Permissions.CONSULTAR })]
        public ActionResult PontoEntregaCoordenadaByOrdem(string NumeroOrdem)
        {
            return Json(new MetodosPontoEntregaView().CoordenadasPontoEntregaOrdem(NumeroOrdem), JsonRequestBehavior.AllowGet);
        }
    }
}