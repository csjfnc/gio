using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web.Mvc;
using Website.BLL.Entities;
using Website.BLL.Enums;
using Website.BLL.Utils.Geocoding;
using Website.DAL.UnitOfWork;
using Website.Identity.CustomAutorizes;
using Website.MVC.Helpers.CustomAttribute;
using Website.MVC.Helpers.CustomController;
using Website.MVC.Models.Maps;

namespace Website.MVC.Controllers
{
    public class AjaxRedeController : BaseController
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
        public ActionResult GetCidades()
        {
            return Json(UnitOfWork.CidadeRepository.Get(c => c.IdCidade > 0).ToList().OrderBy(c => c.Nome), JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        [AppAuthorize(Modulos = new Modules[] { Modules.REDE }, Permissoes = new Permissions[] { Permissions.CONSULTAR })]
        public ActionResult GetIluminacaoPublicaByPoste(long IdPoste)
        {
            List<IP> lstIP = UnitOfWork.IPRepository.Get(ip => ip.Poste.IdPoste == IdPoste && ip.DataExclusao == null).ToList();

            return Json(lstIP, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        [AppAuthorize(Modulos = new Modules[] { Modules.REDE }, Permissoes = new Permissions[] { Permissions.CONSULTAR })]
        public ActionResult GetOsbyCidade(long idCidade)
        {
            List<OrdemDeServico> ListaOs = UnitOfWork.OrdemDeServicoRepository.Get(os => os.Cidade.IdCidade == idCidade, includeProperties: "Cidade").ToList();

            return Json(ListaOs.Select(os => new { os.IdOrdemDeServico, os.NumeroOS }), JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        [AppAuthorize(Modulos = new Modules[] { Modules.REDE }, Permissoes = new Permissions[] { Permissions.CONSULTAR })]
        public ActionResult GetIluminacaoPublicaById(long IdIP, long IdPoste)
        {
            IP IPublica = UnitOfWork.IPRepository.Get(ip => ip.IdIp == IdIP).FirstOrDefault();

            List<FotoPoste> fotosPoste = UnitOfWork.FotoPosteRepository.Get(f => f.IdPoste == IdPoste && f.DataExclusao == null).ToList();
            return Json(new { IPublica = IPublica, fotos = fotosPoste.Select(f => new { f.NumeroFoto, f.IdFotoPoste }) }, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        [AppAuthorize(Modulos = new Modules[] { Modules.REDE }, Permissoes = new Permissions[] { Permissions.CONSULTAR })]
        public ActionResult ExisteIPbyPoste(long IdPoste)
        {
            bool ExisteIp = false;
            bool Finalizado = false;

            if (UnitOfWork.IPRepository.Get(ip => ip.Poste.IdPoste == IdPoste && ip.DataExclusao == null).ToList().Count > 0)
                ExisteIp = true;

            if (UnitOfWork.PosteRepository.Get(p => p.IdPoste == IdPoste).FirstOrDefault().Finalizado)
                Finalizado = true;

            return Json(new { ExisteIp, Finalizado }, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Consulta paginada de postes filtradas por cidade.
        /// </summary>
        /// <param name="idCidade"></param>
        /// <param name="page"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        [HttpGet]
        [AppAuthorize(Modulos = new Modules[] { Modules.REDE }, Permissoes = new Permissions[] { Permissions.CONSULTAR })]
        public ActionResult GetPostesByCidade(long idCidade)
        {
            IEnumerable<Poste> Postes = UnitOfWork.PosteRepository.Get(
                p => p.IdCidade == idCidade &&
                p.DataExclusao == null/* && 
                p.TipoPoste != TipoPoste.ARVORE_P && 
                p.TipoPoste != TipoPoste.ARVORE_M && 
                p.TipoPoste != TipoPoste.ARVORE_G*/,
                includeProperties: "Cidade");
            if (Postes != null && Postes.Count() > 0)
                return SendBigJson(new
                {
                    Postes = PostePaginadoView.Generate(Postes),
                    Limites = new LimitesCidadeView().LimitesByCidade(UnitOfWork.LimiteCidadeRepository.Get(l => l.IdCidade == idCidade, includeProperties: "Cidade").ToList())
                }, JsonRequestBehavior.AllowGet);
            else
                return Json(new { Msg = Resources.Messages.Not_Register_Data_Base }, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        [AppAuthorize(Modulos = new Modules[] { Modules.REDE }, Permissoes = new Permissions[] { Permissions.CONSULTAR })]
        public ActionResult GetDemandasByCidade(long idCidade)
        {
            IEnumerable<PontoEntrega> PontoEntregas = UnitOfWork.PontoEntregaRepository.Get(
                p => p.IdCidade == idCidade &&
                p.DataExclusao == null/* && 
                p.TipoPoste != TipoPoste.ARVORE_P && 
                p.TipoPoste != TipoPoste.ARVORE_M && 
                p.TipoPoste != TipoPoste.ARVORE_G*/,
                includeProperties: "Cidade");
            if (PontoEntregas != null && PontoEntregas.Count() > 0)
                return SendBigJson(new
                {
                    PontoEntregas = PontoPaginadoView.GenerateByOs(PontoEntregas),
                    //Limites = new LimitesCidadeView().LimitesByCidade(UnitOfWork.LimiteCidadeRepository.Get(l => l.IdCidade == idCidade, includeProperties: "Cidade").ToList())
                }, JsonRequestBehavior.AllowGet);
            else
                return Json(new { Msg = Resources.Messages.Not_Register_Data_Base }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetStrands(long idCidade)
        {
            IEnumerable<DemandaStrand> DemandaStrands = UnitOfWork.DemandaStrandRepository.Get(
                s => s.IdCidade == idCidade && s.DataExclusao == null, includeProperties: "Cidade");

            if (DemandaStrands.Count()  > 0)
            {
                return SendBigJson(new
                {
                    DemandaStrands = StrandPaginadoView.Generate(DemandaStrands),
                }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(new { Msg = Resources.Messages.Not_Register_Data_Base }, JsonRequestBehavior.AllowGet);
            }
        }

        /// <summary>
        /// Consulta de poste por OS
        /// </summary>
        /// <param name="codOs"></param>
        /// <returns></returns>
        [HttpGet]
        [AppAuthorize(Modulos = new Modules[] { Modules.REDE }, Permissoes = new Permissions[] { Permissions.CONSULTAR })]
        public ActionResult GetPostesByOs(string codOs)
        {
            IEnumerable<Poste> postes_bd = UnitOfWork.PosteRepository.Get(
                p => p.OrdemDeServico.NumeroOS == codOs &&
                p.DataExclusao == null/* &&
                p.TipoPoste != TipoPoste.ARVORE_P &&
                p.TipoPoste != TipoPoste.ARVORE_M &&
                p.TipoPoste != TipoPoste.ARVORE_G*/,
                includeProperties: "OrdemDeServico,Cidade");

            OrdemDeServico Ordem = UnitOfWork.OrdemDeServicoRepository.Get(o => o.NumeroOS == codOs, includeProperties: "Cidade,PoligonosOS").FirstOrDefault();

            var Informacao = new
            {
                IdCidade = Ordem.Cidade.IdCidade,
                NomeCidade = Ordem.Cidade.Nome
            };


            return Json(new
            {
                Informacao,
                Postes = postes_bd.Count() > 0 ? PostePaginadoView.GenerateByOs(postes_bd) : null,
                Limites = new LimitesCidadeView().LimitesByOS(Ordem)
            }, JsonRequestBehavior.AllowGet);

        }

        /*  [HttpGet]
          [AppAuthorize(Modulos = new Modules[] {Modules.REDE}, Permissions = new Permissions[] {Permissions.CONSULTAR})]
          public ActionResult GetDemandaByOs(string codOs)
          {
              IEnumerable<PontoEntrega> pontoDemanda_bd = UnitOfWork.PontoEntregaRepository.Get(
                  p => p.OrdemDeServico.NumeroOS == codOs && p.DataExclusao == null,
                      includeProperties: "OrdemDeServico");

              if (pontoDemanda_bd != null && pontoDemanda_bd.Count() > 0)
              {
                  return Json(new
                  {
                      Ponte
                  });
              }
          }*/

        /// <summary>
        /// Consulta paginada de postes filtradas por cidade e filtro by Potencia.
        /// </summary>
        /// <param name="idCidade"></param>
        /// <param name="page"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        [HttpGet]
        [AppAuthorize(Modulos = new Modules[] { Modules.REDE }, Permissoes = new Permissions[] { Permissions.CONSULTAR })]
        public ActionResult GetPostesByCidadePotencia(long idCidade)
        {
            IEnumerable<Poste> Postes = UnitOfWork.PosteRepository.Get(
                p => p.IdCidade == idCidade &&
                p.DataExclusao == null/* &&
                p.TipoPoste != TipoPoste.ARVORE_P &&
                p.TipoPoste != TipoPoste.ARVORE_M &&
                p.TipoPoste != TipoPoste.ARVORE_G*/, includeProperties: "Cidade,IP");
            if (Postes != null && Postes.Count() > 0)
                return SendBigJson(PostePaginadoView.GeneratePotencia(Postes), JsonRequestBehavior.AllowGet);
            else
                return Json(new { Msg = Resources.Messages.Not_Register_Data_Base }, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Consulta de poste por OS com o filtro Potencia
        /// </summary>
        /// <param name="codOs"></param>
        /// <returns></returns>
        [HttpGet]
        [AppAuthorize(Modulos = new Modules[] { Modules.REDE }, Permissoes = new Permissions[] { Permissions.CONSULTAR })]
        public ActionResult GetPostesByOsPotencia(string codOs)
        {
            IEnumerable<Poste> postes = UnitOfWork.PosteRepository.Get(
                p => p.OrdemDeServico.NumeroOS == codOs &&
                p.DataExclusao == null/* &&
                p.TipoPoste != TipoPoste.ARVORE_P &&
                p.TipoPoste != TipoPoste.ARVORE_M &&
                p.TipoPoste != TipoPoste.ARVORE_G*/, includeProperties: "OrdemDeServico,Cidade,Ip");

            if (postes != null && postes.Count() > 0)
                return Json(PostePaginadoView.GenerateByOsPotencia(postes), JsonRequestBehavior.AllowGet);
            else
                return Json(new { Msg = Resources.Messages.Not_Register_Data_Base }, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Consulta paginada de postes filtradas por cidade e filtro by Lampada.
        /// </summary>
        /// <param name="idCidade"></param>
        /// <param name="page"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        [HttpGet]
        [AppAuthorize(Modulos = new Modules[] { Modules.REDE }, Permissoes = new Permissions[] { Permissions.CONSULTAR })]
        public ActionResult GetPostesByCidadeLampada(long idCidade)
        {
            IEnumerable<Poste> Postes = UnitOfWork.PosteRepository.Get(
                p => p.IdCidade == idCidade &&
                p.DataExclusao == null/* &&
                p.TipoPoste != TipoPoste.ARVORE_P &&
                p.TipoPoste != TipoPoste.ARVORE_M &&
                p.TipoPoste != TipoPoste.ARVORE_G*/, includeProperties: "Cidade,IP");
            if (Postes != null && Postes.Count() > 0)
                return SendBigJson(PostePaginadoView.GenerateLampada(Postes), JsonRequestBehavior.AllowGet);
            else
                return Json(new { Msg = Resources.Messages.Not_Register_Data_Base }, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Consulta de poste por OS com o filtro Lampada
        /// </summary>
        /// <param name="codOs"></param>
        /// <returns></returns>
        [HttpGet]
        [AppAuthorize(Modulos = new Modules[] { Modules.REDE }, Permissoes = new Permissions[] { Permissions.CONSULTAR })]
        public ActionResult GetPostesByOsLampada(string codOs)
        {
            IEnumerable<Poste> postes = UnitOfWork.PosteRepository.Get(
                p => p.OrdemDeServico.NumeroOS == codOs &&
                p.DataExclusao == null/* &&
                p.TipoPoste != TipoPoste.ARVORE_P &&
                p.TipoPoste != TipoPoste.ARVORE_M &&
                p.TipoPoste != TipoPoste.ARVORE_G*/, includeProperties: "OrdemDeServico,Cidade,Ip");

            if (postes != null && postes.Count() > 0)
                return Json(PostePaginadoView.GenerateByOsLampada(postes), JsonRequestBehavior.AllowGet);
            else
                return Json(new { Msg = Resources.Messages.Not_Register_Data_Base }, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Consulta paginada de postes filtradas por cidade e filtro by Lampada.
        /// </summary>
        /// <param name="idCidade"></param>
        /// <param name="page"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        [HttpGet]
        [AppAuthorize(Modulos = new Modules[] { Modules.REDE }, Permissoes = new Permissions[] { Permissions.CONSULTAR })]
        public ActionResult GetPostesByCidadeStatus(long idCidade)
        {
            IEnumerable<Poste> Postes = UnitOfWork.PosteRepository.Get(p => p.IdCidade == idCidade && p.DataExclusao == null, includeProperties: "Cidade,IP");
            if (Postes != null && Postes.Count() > 0)
                return SendBigJson(PostePaginadoView.GenerateStatus(Postes), JsonRequestBehavior.AllowGet);
            else
                return Json(new { Msg = Resources.Messages.Not_Register_Data_Base }, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Consulta de poste por OS com o filtro Lampada
        /// </summary>
        /// <param name="codOs"></param>
        /// <returns></returns>
        [HttpGet]
        [AppAuthorize(Modulos = new Modules[] { Modules.REDE }, Permissoes = new Permissions[] { Permissions.CONSULTAR })]
        public ActionResult GetPostesByOsStatus(string codOs)
        {
            IEnumerable<Poste> postes = UnitOfWork.PosteRepository.Get(
                p => p.OrdemDeServico.NumeroOS == codOs &&
                p.DataExclusao == null/* &&
                p.TipoPoste != TipoPoste.ARVORE_P &&
                p.TipoPoste != TipoPoste.ARVORE_M &&
                p.TipoPoste != TipoPoste.ARVORE_G*/, includeProperties: "OrdemDeServico,Cidade,IP");

            if (postes != null && postes.Count() > 0)
                return Json(PostePaginadoView.GenerateByOsStatus(postes), JsonRequestBehavior.AllowGet);
            else
                return Json(new { Msg = Resources.Messages.Not_Register_Data_Base }, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        [AppAuthorize(Modulos = new Modules[] { Modules.REDE }, Permissoes = new Permissions[] { Permissions.CONSULTAR })]
        public ActionResult GetPostesByCidadeNaSa(long idCidade)
        {
            IEnumerable<Poste> Postes = UnitOfWork.PosteRepository.Get(
                p => p.IdCidade == idCidade &&
                p.DataExclusao == null/* &&
                p.TipoPoste != TipoPoste.ARVORE_P &&
                p.TipoPoste != TipoPoste.ARVORE_M &&
                p.TipoPoste != TipoPoste.ARVORE_G*/, includeProperties: "Cidade,Fotos");
            if (Postes != null && Postes.Count() > 0)
                return SendBigJson(PostePaginadoView.GenerateNaSa(Postes), JsonRequestBehavior.AllowGet);
            else
                return Json(new { Msg = Resources.Messages.Not_Register_Data_Base }, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        [AppAuthorize(Modulos = new Modules[] { Modules.REDE }, Permissoes = new Permissions[] { Permissions.CONSULTAR })]
        public ActionResult GetPostesByOsNaSa(string codOs)
        {
            IEnumerable<Poste> postes = UnitOfWork.PosteRepository.Get(
                p => p.OrdemDeServico.NumeroOS == codOs &&
                p.DataExclusao == null/* &&
                p.TipoPoste != TipoPoste.ARVORE_P &&
                p.TipoPoste != TipoPoste.ARVORE_M &&
                p.TipoPoste != TipoPoste.ARVORE_G*/, includeProperties: "OrdemDeServico,Cidade,Fotos");

            if (postes != null && postes.Count() > 0)
                return Json(PostePaginadoView.GenerateByOsNaSa(postes), JsonRequestBehavior.AllowGet);
            else
                return Json(new { Msg = Resources.Messages.Not_Register_Data_Base }, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        [AppAuthorize(Modulos = new Modules[] { Modules.REDE }, Permissoes = new Permissions[] { Permissions.CONSULTAR })]
        public ActionResult GetPosteByIdFiltroLampada(long IdPoste)
        {
            PosteView Poste = new MetodosPosteView().PostetoPosteViewFiltroLampada(UnitOfWork.PosteRepository.Get(
                p => p.IdPoste == IdPoste &&
                p.DataExclusao == null/* &&
                p.TipoPoste != TipoPoste.ARVORE_P &&
                p.TipoPoste != TipoPoste.ARVORE_M &&
                p.TipoPoste != TipoPoste.ARVORE_G*/, includeProperties: "Cidade,IP").FirstOrDefault());
            return Json(Poste, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        [AppAuthorize(Modulos = new Modules[] { Modules.REDE }, Permissoes = new Permissions[] { Permissions.CONSULTAR })]
        public ActionResult GetPosteByIdFiltroPotencia(long IdPoste)
        {
            PosteView Poste = new MetodosPosteView().PostetoPosteViewFiltroPotencia(UnitOfWork.PosteRepository.Get(
                p => p.IdPoste == IdPoste &&
                p.DataExclusao == null/* &&
                p.TipoPoste != TipoPoste.ARVORE_P &&
                p.TipoPoste != TipoPoste.ARVORE_M &&
                p.TipoPoste != TipoPoste.ARVORE_G*/, includeProperties: "Cidade,IP").FirstOrDefault());
            return Json(Poste, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        [AppAuthorize(Modulos = new Modules[] { Modules.REDE }, Permissoes = new Permissions[] { Permissions.CONSULTAR })]
        public ActionResult GetPosteById(long IdPoste)
        {
            Poste poste = UnitOfWork.PosteRepository.Get(p => p.IdPoste == IdPoste && p.DataExclusao == null, includeProperties: "Cidade").FirstOrDefault();
            FotoPoste foto = UnitOfWork.FotoPosteRepository.Get(f => f.IdPoste == IdPoste && f.DataExclusao == null).FirstOrDefault();

            PosteView posteview = new MetodosPosteView().PostetoPosteView(poste);

            LatLon LatLong = new ConverterUtmToLatLon(poste.Cidade.Datum, poste.Cidade.NorteOuSul, poste.Cidade.Zona).Convert(poste.X, poste.Y);

            //Object poste_return = new
            //{
            //    IdPoste = poste.IdPoste,
            //    Latitude = LatLong.Lat,
            //    Longitude = LatLong.Lon,
            //    //Img = poste.Finalizado == true ? "03" : "08",
            //    Img = poste.Finalizado == true ? "10" : "08",
            //    IdCidade = poste.IdCidade,
            //    CodGeo = poste.CodigoGeo,
            //    Altura = poste.Altura,
            //    TipoPoste = poste.TipoPoste,
            //    Esforco = poste.Esforco,
            //    Descricao = poste.Descricao
            //};

            return Json(posteview, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        [AppAuthorize(Modulos = new Modules[] { Modules.REDE }, Permissoes = new Permissions[] { Permissions.CONSULTAR })]
        public ActionResult GetPontoEntregaById(long idPontoEntrega)
        {
            PontoEntrega pontoEntrega = UnitOfWork.PontoEntregaRepository.Get(p => p.IdPontoEntrega == idPontoEntrega && p.DataExclusao == null, includeProperties: "Cidade").FirstOrDefault();
            FotoPontoEntrega foto = UnitOfWork.FotoPontoEntregaRepository.Get(f => f.IdPontoEntrega == idPontoEntrega && f.DataExclusao == null).FirstOrDefault();

            PontoEntregaView pontoEntregaView = new MetodosPontoEntregaView().PontoEntregaToPontoEntregaView(pontoEntrega);

            LatLon LatLong = new ConverterUtmToLatLon(pontoEntrega.Cidade.Datum, pontoEntrega.Cidade.NorteOuSul, pontoEntrega.Cidade.Zona).Convert(pontoEntrega.X, pontoEntrega.Y);

            //Object poste_return = new
            //{
            //    IdPoste = poste.IdPoste,
            //    Latitude = LatLong.Lat,
            //    Longitude = LatLong.Lon,
            //    //Img = poste.Finalizado == true ? "03" : "08",
            //    Img = poste.Finalizado == true ? "10" : "08",
            //    IdCidade = poste.IdCidade,
            //    CodGeo = poste.CodigoGeo,
            //    Altura = poste.Altura,
            //    TipoPoste = poste.TipoPoste,
            //    Esforco = poste.Esforco,
            //    Descricao = poste.Descricao
            //};

            return Json(pontoEntregaView, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        [AppAuthorize(Modulos = new Modules[] { Modules.REDE }, Permissoes = new Permissions[] { Permissions.CONSULTAR })]
        public ActionResult GetFotosByPoste(int IdPoste)
        {
            List<FotoPoste> fotos = UnitOfWork.FotoPosteRepository.Get(f => f.IdPoste == IdPoste && f.DataExclusao == null).ToList();
            List<FotoView> fotos_view = new List<FotoView>();

            foreach (FotoPoste foto_bd in fotos)
            {
                fotos_view.Add(new FotoView() { IdFotoPoste = foto_bd.IdFotoPoste, DataFoto = string.Format("{0:yyyy-MM-dd}", foto_bd.DataFoto), NumeroFoto = foto_bd.NumeroFoto });
            }

            return Json(fotos_view, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        [AppAuthorize(Modulos = new Modules[] { Modules.REDE }, Permissoes = new Permissions[] { Permissions.CONSULTAR })]
        public ActionResult GetFotosByPontoEntrega(int idPontoEntrega)
        {
            List<FotoPontoEntrega> fotos = UnitOfWork.FotoPontoEntregaRepository.Get(f => f.IdPontoEntrega == idPontoEntrega && f.DataExclusao == null).ToList();
            List<FotoPontoEntregaView> fotos_view = new List<FotoPontoEntregaView>();

            foreach (FotoPontoEntrega foto_bd in fotos)
            {
                fotos_view.Add(new FotoPontoEntregaView()
                {
                    IdFotoPontoEntrega = foto_bd.IdFotoPontoEntrega,
                    DataFoto = string.Format("{0:yyyy-MM-dd}", foto_bd.DataFoto),
                    NumeroFoto = foto_bd.NumeroFoto
                });
            }

            return Json(fotos_view, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        [AppAuthorize(Modulos = new Modules[] { Modules.REDE }, Permissoes = new Permissions[] { Permissions.ATUALIZAR })]
        public ActionResult SalvarEdicaoIP(int _IdPosteAssociado, int _IdIp, string _TipoBraco, string _TipoLuminaria, string _TipoLampada, int _QtdLuminaria, double _Potencia, string _Acionamento, string _LampadaAcesa, string _Fase, int _QtdLampada)
        {
            if (_IdIp == -1)
            {
                Poste poste = UnitOfWork.PosteRepository.Get(p => p.IdPoste == _IdPosteAssociado && p.DataExclusao == null).FirstOrDefault();
                UnitOfWork.IPRepository.Insert(new IP() { Poste = poste, TipoBraco = _TipoBraco, TipoLuminaria = _TipoLuminaria, QtdLuminaria = _QtdLuminaria, TipoLampada = _TipoLampada, Potencia = _Potencia, CodigoGeoBD = -1, Acionamento = _Acionamento, LampadaAcesa = _LampadaAcesa, Fase = _Fase, QtdLampada = _QtdLampada });
                UnitOfWork.Save();
            }
            else
            {
                IP Ip = UnitOfWork.IPRepository.Get(i => i.IdIp == _IdIp && i.DataExclusao == null).FirstOrDefault();

                Ip.IdPoste = _IdPosteAssociado;
                Ip.TipoBraco = _TipoBraco;
                Ip.TipoLuminaria = _TipoLuminaria;
                Ip.TipoLampada = _TipoLampada;
                Ip.QtdLuminaria = _QtdLuminaria;
                Ip.Potencia = _Potencia;
                Ip.Acionamento = _Acionamento;
                Ip.Fase = _Fase;
                Ip.LampadaAcesa = _LampadaAcesa;
                Ip.QtdLampada = _QtdLampada;

                UnitOfWork.IPRepository.Update(Ip);
                UnitOfWork.Save();
            }

            return Json(new ResponseView() { Status = Status.OK, Result = Resources.Messages.Save_OK }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        [AppAuthorize(Modulos = new Modules[] { Modules.REDE }, Permissoes = new Permissions[] { Permissions.REMOVER })]
        public ActionResult ExcluirIpById(long IdIp)
        {
            IP ip = UnitOfWork.IPRepository.Get(x => x.IdIp == IdIp && x.DataExclusao == null).FirstOrDefault();
            if (ip != null)
            {
                ip.DataExclusao = DateTime.Now;
                UnitOfWork.IPRepository.Update(ip);
                UnitOfWork.Save();

                return Json(new ResponseView() { Status = Status.OK, Result = Resources.Messages.Save_OK }, JsonRequestBehavior.AllowGet);
            }
            else
                return Json(new ResponseView() { Status = Status.NotFound, Result = Resources.Messages.Ip_Not_Found }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        [AppAuthorize(Modulos = new Modules[] { Modules.REDE }, Permissoes = new Permissions[] { Permissions.ATUALIZAR })]
        public ActionResult SalvarEdicaoPoste(PosteJs objeto_poste)
        {
            Poste posteBd = UnitOfWork.PosteRepository.Get(pp => pp.IdPoste == objeto_poste.IdPoste && pp.DataExclusao == null, includeProperties: "OrdemDeServico,Cidade").FirstOrDefault();

            if (posteBd != null)
            {
                List<FotoPoste> fotosPoste = UnitOfWork.FotoPosteRepository.Get(f => f.IdPoste == objeto_poste.IdPoste).ToList();

                foreach (FotoPoste f in fotosPoste)
                {
                    UnitOfWork.FotoPosteRepository.Delete(f);
                }

                if (objeto_poste.LstFoto != null && objeto_poste.LstFoto.Count > 0)
                {
                    foreach (FotoView foto_view in objeto_poste.LstFoto)
                    {
                        if (!String.IsNullOrEmpty(foto_view.NumeroFoto.Trim()))
                        {
                            /// Nao deve Vim foto_view.DataFoto vazio mas se vim nao quebra o codigo
                            DateTime DataDiretorio = foto_view.DataFoto != String.Empty ? Convert.ToDateTime(foto_view.DataFoto) : DateTime.Now;
                            String Data = DataDiretorio.ToString("ddMMyyyy");

                            FotoPoste ft = new FotoPoste
                            {
                                CodigoGeoBD = -1,
                                IdPoste = objeto_poste.IdPoste,
                                NumeroFoto = foto_view.NumeroFoto.Trim().ToUpper(),
                                DataFoto = DataDiretorio,
                                Path = string.Format(ConfigurationManager.AppSettings["NewPathFotos"], posteBd.Cidade.CidadeDiretorio, Data, User.Identity.Name.ToUpper(), foto_view.NumeroFoto.Trim().ToUpper())
                            };

                            UnitOfWork.FotoPosteRepository.Insert(ft);
                        }
                    }
                }

               // ConverterLatLonToUtm converter = new ConverterLatLonToUtm(posteBd.Cidade.Datum, posteBd.Cidade.NorteOuSul, posteBd.Cidade.Zona);
               // UTM utm = converter.Convert(objeto_poste.Latitude, objeto_poste.Longitude);

               // posteBd.X = utm.X;
               // posteBd.Y = utm.Y;

                posteBd.Altura = objeto_poste.Altura;
                posteBd.Esforco = objeto_poste.Esforco != null ? objeto_poste.Esforco : 0;
                posteBd.TipoPoste = objeto_poste.TipoPoste;
                posteBd.encontrado = objeto_poste.EncontradoPoste; 
                posteBd.para_raio = objeto_poste.PararioPoste;
                posteBd.aterramento = objeto_poste.AterramentoPoste;
                posteBd.estrutura_primaria = objeto_poste.EstruturaPrimariaPoste;
                posteBd.estrutura_secundaria = objeto_poste.EstruturaSecundaria_poste;
                posteBd.qtd_estai = objeto_poste.QuantidadeEstai;
                posteBd.ano = objeto_poste.AnoPoste;
                posteBd.situacao = objeto_poste.SituacaoPoste;
                posteBd.equipamento1 = objeto_poste.EquipamentoPoste;
                posteBd.mufla = objeto_poste.MuflaPoste;
                posteBd.rede_primaria = objeto_poste.RedePrimarioPoste;
                posteBd.defeito = objeto_poste.DefeitoPoste;
                posteBd.barramento = objeto_poste.BarramentoPoste;

                //posteBd.Descricao = objeto_poste.Descricao != null ? objeto_poste.Descricao.ToUpper() : "";
                posteBd.DataCadastro = DateTime.Now;

                /// Atualizando o Poste
                UnitOfWork.PosteRepository.Update(posteBd);

                //Salvando as Alteraçoes
                UnitOfWork.Save();

                return Json(new ResponseView() { Status = Status.OK, Result = Resources.Messages.Save_OK }, JsonRequestBehavior.AllowGet);
            }
            else
                return Json(new ResponseView() { Status = Status.NotFound, Result = Resources.Messages.Poste_Not_Found }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        [AppAuthorize(Modulos = new Modules[] { Modules.REDE }, Permissoes = new Permissions[] { Permissions.REMOVER })]
        public ActionResult ExcluirPoste(int IdPoste)
        {
            Poste poste = UnitOfWork.PosteRepository.Get(p => p.IdPoste == IdPoste && p.DataExclusao == null).FirstOrDefault();
            if (poste != null)
            {
                poste.DataExclusao = DateTime.Now;
                UnitOfWork.PosteRepository.Update(poste);
                UnitOfWork.Save();

                return Json(new ResponseView() { Status = Status.OK, Result = Resources.Messages.Save_OK }, JsonRequestBehavior.AllowGet);
            }
            else
                return Json(new ResponseView() { Status = Status.NotFound, Result = Resources.Messages.Poste_Not_Found }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        [AppAuthorize(Modulos = new Modules[] { Modules.REDE }, Permissoes = new Permissions[] { Permissions.REMOVER })]
        public ActionResult ExcluirDemanda(long idDemanda)
        {
            PontoEntrega pontoEntrega_bd = UnitOfWork.PontoEntregaRepository.Get(p => p.IdPontoEntrega == idDemanda && p.DataExclusao == null).FirstOrDefault();
            VaosDemandaPoste vaosDemandaPoste_bd = UnitOfWork.VaosDemandaPosteRepository.Get(v => v.IdPontoEntrega == idDemanda && pontoEntrega_bd.DataExclusao == null).FirstOrDefault();

            if (vaosDemandaPoste_bd != null)
            {
                vaosDemandaPoste_bd.DataExclusao = DateTime.Now;
                UnitOfWork.VaosDemandaPosteRepository.Update(vaosDemandaPoste_bd);
                UnitOfWork.Save();
            }
            if (pontoEntrega_bd != null)
            {
                pontoEntrega_bd.DataExclusao = DateTime.Now;
                UnitOfWork.PontoEntregaRepository.Update(pontoEntrega_bd);
                UnitOfWork.Save();

                return Json(new ResponseView() { Status = Status.OK, Result = Resources.Messages.Save_OK }, JsonRequestBehavior.AllowGet);

            }
            else
                return Json(new ResponseView() { Status = Status.NotFound, Result = Resources.Messages.Poste_Not_Found }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        [AppAuthorize(Modulos = new Modules[] { Modules.REDE }, Permissoes = new Permissions[] { Permissions.ADICIONAR })]
        public ActionResult NewPoste(PosteView requestData)
        {
            Cidade cidade = UnitOfWork.CidadeRepository.Get(c => c.IdCidade == requestData.IdCidade).FirstOrDefault();
            if (cidade != null)
            {

                ConverterLatLonToUtm converter = new ConverterLatLonToUtm(cidade.Datum, cidade.NorteOuSul, cidade.Zona);
                UTM utm = converter.Convert(requestData.Latitude, requestData.Longitude);

                OrdemDeServico ordemDeServico = UnitOfWork.OrdemDeServicoRepository.Get(or => or.NumeroOS == requestData.IdOrdemServicoTexto).FirstOrDefault();

                Poste p = new Poste { X = utm.X, Y = utm.Y, Cidade = cidade, DataCadastro = DateTime.Now, IdOrdemDeServico = ordemDeServico.IdOrdemDeServico, CodigoGeo = -1 };

                UnitOfWork.PosteRepository.Insert(p);
                UnitOfWork.Save();

                Poste pst = UnitOfWork.PosteRepository.Get(pt => pt.X == p.X && pt.Y == p.Y, includeProperties: "Cidade").FirstOrDefault();

                PosteView posteview = new MetodosPosteView().PostetoPosteView(pst);

                return Json(new ResponseView() { Status = Status.Found, Result = posteview }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(new ResponseView() { Status = Status.NotFound, Result = Resources.Messages.Cidade_Not_Found }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        [AppAuthorize(Modulos = new Modules[] { Modules.REDE }, Permissoes = new Permissions[] { Permissions.ADICIONAR })]
        public ActionResult NewAnotacao(AnotacaoView requestData)
        {
            Cidade cidade = UnitOfWork.CidadeRepository.Get(c => c.IdCidade == requestData.IdCidade).FirstOrDefault();
            OrdemDeServico ordemDeServico = UnitOfWork.OrdemDeServicoRepository.Get(or => or.NumeroOS == requestData.NumeroOs).FirstOrDefault();

            if (cidade != null)
            {
                ConverterLatLonToUtm converter = new ConverterLatLonToUtm(cidade.Datum, cidade.NorteOuSul, cidade.Zona);
                UTM utm = converter.Convert(requestData.X, requestData.Y);

                Anotacao anotacao = new Anotacao
                {
                    IdCidade = cidade.IdCidade,
                    IdOrdemDeServico = ordemDeServico.IdOrdemDeServico,
                    Descricao = requestData.Descricao,
                    X = utm.X,
                    Y = utm.Y
                };
                UnitOfWork.AnotacaoRepository.Insert(anotacao);
                UnitOfWork.Save();

                Anotacao anotacaoview = UnitOfWork.AnotacaoRepository.Get(nota => nota.IdAnotacao == anotacao.IdAnotacao).FirstOrDefault();
                AnotacaoView AnotacaoView = new AnotacaoPaginadoView().AnotacaToAnotacaoView(anotacaoview);

              /*  var notas = new
                {
                    IdCidade = anotacaoview.IdCidade,
                    IdOrdemDeServico = anotacaoview.IdOrdemDeServico,
                    Descricao = anotacaoview.Descricao,
                    X = anotacaoview.X,
                    Y = anotacaoview.Y
                };*/

                return Json(new ResponseView() { Status = Status.Found, Result = AnotacaoView }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(new ResponseView() { Status = Status.NotFound, Result = "Cidade nao Encontrada" }, JsonRequestBehavior.AllowGet);
            }

        }

        [HttpPost]
        [AppAuthorize(Modulos = new Modules[] { Modules.REDE }, Permissoes = new Permissions[] { Permissions.ATUALIZAR })]
        public ActionResult FinalizarPoste(long IdPoste)
        {
            Poste poste = UnitOfWork.PosteRepository.Get(p => p.IdPoste == IdPoste && p.DataExclusao == null).FirstOrDefault();
            if (poste != null)
            {
                poste.Finalizado = true;
                poste.DataFinalizado = DateTime.Now;

                UnitOfWork.PosteRepository.Update(poste);
                UnitOfWork.Save();

                return Json(new ResponseView() { Status = Status.OK, Result = Resources.Messages.Save_OK }, JsonRequestBehavior.AllowGet);
            }
            else
                return Json(new ResponseView() { Status = Status.NotFound, Result = Resources.Messages.Poste_Not_Found }, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        [AppAuthorize(Modulos = new Modules[] { Modules.REDE }, Permissoes = new Permissions[] { Permissions.ATUALIZAR })]
        public ActionResult SalvarArrastarPoste(PosteJs objeto_poste)
        {
            Poste posteBd = UnitOfWork.PosteRepository.Get(pp => pp.IdPoste == objeto_poste.IdPoste && pp.DataExclusao == null, includeProperties: "OrdemDeServico,Cidade").FirstOrDefault();
            var vaos = UnitOfWork.VaosDemandaPosteRepository.Get(v => v.IdPoste == objeto_poste.IdPoste).ToList();
            if (posteBd != null)
            {

                ConverterLatLonToUtm converter = new ConverterLatLonToUtm(posteBd.Cidade.Datum, posteBd.Cidade.NorteOuSul, posteBd.Cidade.Zona);
                UTM utm = converter.Convert(objeto_poste.Latitude, objeto_poste.Longitude);

                posteBd.X = utm.X;
                posteBd.Y = utm.Y;
                //posteBd.Esforco = objeto_poste.Esforco != null ? objeto_poste.Esforco.ToUpper() : "";
                //posteBd.TipoPoste = objeto_poste.TipoPoste;
                //posteBd.Altura = objeto_poste.Altura;
                //posteBd.Descricao = objeto_poste.Descricao != null ? objeto_poste.Descricao.ToUpper() : "";
                //posteBd.DataCadastro = DateTime.Now;

                /// Atualizando o Poste
                UnitOfWork.PosteRepository.Update(posteBd);

                //Salvando as Alteraçoes
                UnitOfWork.Save();

                Poste poste = UnitOfWork.PosteRepository.Get(p => p.IdPoste == objeto_poste.IdPoste).FirstOrDefault();

                if (vaos != null)
                {
                    foreach (var item in vaos)
                    {
                        item.X1 = utm.X;
                        item.Y1 = utm.Y;
                    }
                }

                UnitOfWork.Save();
                 
                return Json(new 
                { 
                   Poste =  PostePaginadoView.GeneratePosteUnico(poste),
                   VaosDemandas = VaosDemandasPaginadoView.GenerateByOs(vaos) 
                }, JsonRequestBehavior.AllowGet);
            }
            else
                return Json(new ResponseView() { Status = Status.NotFound, Result = Resources.Messages.Poste_Not_Found }, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        [AppAuthorize(Modulos = new Modules[] { Modules.REDE }, Permissoes = new Permissions[] { Permissions.ATUALIZAR })]
        public ActionResult CancelfinalizarPoste(long IdPoste)
        {
            Poste poste = UnitOfWork.PosteRepository.Get(p => p.IdPoste == IdPoste && p.DataExclusao == null).FirstOrDefault();
            if (poste != null)
            {
                poste.Finalizado = false;
                poste.DataFinalizado = null;

                UnitOfWork.PosteRepository.Update(poste);
                UnitOfWork.Save();

                return Json(new ResponseView() { Status = Status.OK, Result = Resources.Messages.Save_OK }, JsonRequestBehavior.AllowGet);
            }
            else
                return Json(new ResponseView() { Status = Status.NotFound, Result = Resources.Messages.Poste_Not_Found }, JsonRequestBehavior.AllowGet);
        }

        protected override void Dispose(bool disposing)
        {
            UnitOfWork.Dispose();
            base.Dispose(disposing);
        }

    }
}