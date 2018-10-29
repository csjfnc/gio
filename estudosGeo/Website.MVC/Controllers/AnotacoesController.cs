using System;
using System.Collections.Generic;
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
    public class AnotacoesController : BaseController
    {

        private readonly UnitOfWork UnitOfWork = new UnitOfWork();
        long IdCidade = -1;
        ConverterUtmToLatLon converter = null;
        string NomeCidade = string.Empty;
        //
        // GET: /Anotacoes/
        public ActionResult GetAnotacaoByOs(string codOs)
        {
            List<Anotacao> Anotacaos = new List<Anotacao>();
            var anotacoes = UnitOfWork.AnotacaoRepository.Get(a => a.OrdemDeServico.NumeroOS == codOs && a.DataExclusao == null, includeProperties: "OrdemDeServico, Cidade").ToList();

            foreach (var item in anotacoes)
            {

                if (converter == null) converter = new ConverterUtmToLatLon(item.OrdemDeServico.Cidade.Datum, item.OrdemDeServico.Cidade.NorteOuSul, item.OrdemDeServico.Cidade.Zona);
                if (IdCidade == -1) IdCidade = item.OrdemDeServico.IdCidade;
                if (NomeCidade == string.Empty) NomeCidade = item.OrdemDeServico.Cidade.Nome;

                LatLon LatiLong1 = converter.Convert(item.X, item.Y);
                Anotacaos.Add(new Anotacao()
                {
                    Descricao = item.Descricao,
                    IdAnotacao = item.IdAnotacao,
                    IdOrdemDeServico = item.IdOrdemDeServico,
                    X = LatiLong1.Lat,
                    Y = LatiLong1.Lon
                });
            }
            return Json(new ResponseView() { Status = Status.OK, Result = Anotacaos }, JsonRequestBehavior.AllowGet);
        }


        [HttpGet]
        [AppAuthorize(Modulos = new Modules[] { Modules.REDE }, Permissoes = new Permissions[] { Permissions.CONSULTAR })]
        public ActionResult GetAnotacaoesByCidade(long idCidade)
        {
            IEnumerable<Anotacao> anotacoes = UnitOfWork.AnotacaoRepository.Get(
                p => p.IdCidade == idCidade && p.DataExclusao == null, includeProperties: "Cidade");

            if (anotacoes != null && anotacoes.Count() > 0)
                return Json(new
                {
                    Anotacoes = AnotacaoPaginadoView.Generate(anotacoes)
                    //Limites = new LimitesCidadeView().LimitesByOS(UnitOfWork.OrdemDeServicoRepository.Get(o => o.NumeroOS == codOs, includeProperties: "Cidade,PoligonosOS").FirstOrDefault())
                }, JsonRequestBehavior.AllowGet);
            else
                return Json(new { Msg = Resources.Messages.Not_Register_Data_Base }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        [AppAuthorize(Modulos = new Modules[] { Modules.REDE }, Permissoes = new Permissions[] { Permissions.REMOVER })]
        public ActionResult ExcluirAnotacao(int idAnotacao)
        {
            Anotacao anotacao = UnitOfWork.AnotacaoRepository.Get(a => a.IdAnotacao == idAnotacao && a.DataExclusao == null).FirstOrDefault();
            if (anotacao != null)
            {
                anotacao.DataExclusao = DateTime.Now;
                UnitOfWork.AnotacaoRepository.Update(anotacao);
                UnitOfWork.Save();

                return Json(new ResponseView() { Status = Status.OK, Result = Resources.Messages.Save_OK }, JsonRequestBehavior.AllowGet);
            }
            else
                return Json(new ResponseView() { Status = Status.NotFound, Result = Resources.Messages.Poste_Not_Found }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult EditarAnotacao(long id, string descricao)
        {
            Anotacao Anotacao = UnitOfWork.AnotacaoRepository.Get(a => a.IdAnotacao == id).FirstOrDefault();
            if (Anotacao != null)
            {
                Anotacao.Descricao = descricao;
                UnitOfWork.Save();
                return Json(new { Status = Status.OK, Result = "OK" }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(new { Status = Status.NotFound, Result = "Erro" }, JsonRequestBehavior.AllowGet);
            }
        }
    }
}