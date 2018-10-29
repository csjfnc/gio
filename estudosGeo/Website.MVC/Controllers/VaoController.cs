using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Website.BLL.Entities;
using Website.DAL.UnitOfWork;
using Website.Identity.CustomAutorizes;
using Website.MVC.Helpers.CustomAttribute;
using Website.MVC.Helpers.CustomController;
using Website.MVC.Models.Maps;

namespace Website.MVC.Controllers
{
    public class VaoController : BaseController
    {
        private readonly UnitOfWork UnitOfWork = new UnitOfWork();

        [HttpGet]
        [AppAuthorize(Modulos = new Modules[] { Modules.REDE }, Permissoes = new Permissions[] { Permissions.CONSULTAR })]
        public ActionResult GetVaosByOs(string NumeroOrdem)
        {
            OrdemDeServico os = UnitOfWork.OrdemDeServicoRepository.Get(o => o.NumeroOS == NumeroOrdem, includeProperties : "Cidade").FirstOrDefault();
            List<VaoPrimario> vaos = UnitOfWork.VaoPrimarioRepository.Get(v => v.IdOrdemDeServico == os.IdOrdemDeServico).ToList();

            return Json(new MetodosVaoView().VaosToVaosViewByOs(vaos, os), JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        [AppAuthorize(Modulos = new Modules[] { Modules.REDE }, Permissoes = new Permissions[] { Permissions.CONSULTAR })]
        public ActionResult GetVaosByCidade(long IdCidade)
        {
            List<OrdemDeServico> ordens = UnitOfWork.OrdemDeServicoRepository.Get(o => o.IdCidade == IdCidade, includeProperties: "Cidade").ToList();
            return Json(new MetodosVaoView().VaosToVaosViewByCidade(ordens), JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        [AppAuthorize(Modulos = new Modules[] { Modules.REDE }, Permissoes = new Permissions[] { Permissions.CONSULTAR })]
        public ActionResult GetVaosDemandasByOs(string codOs)
        {
            IEnumerable<VaosDemandaPoste> vaos_demandas_bd = UnitOfWork.VaosDemandaPosteRepository.Get(
                p => p.OrdemDeServico.NumeroOS == codOs &&
                p.DataExclusao == null, includeProperties:"Cidade,OrdemDeServico");

            if (vaos_demandas_bd != null && vaos_demandas_bd.Count() > 0)
                return Json(new
                {
                    VaosDemandas = VaosDemandasPaginadoView.GenerateByOs(vaos_demandas_bd)
                    //Limites = new LimitesCidadeView().LimitesByOS(UnitOfWork.OrdemDeServicoRepository.Get(o => o.NumeroOS == codOs, includeProperties: "Cidade,PoligonosOS").FirstOrDefault())
                }, JsonRequestBehavior.AllowGet);
            else
                return Json(new { Msg = Resources.Messages.Not_Register_Data_Base }, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        [AppAuthorize(Modulos = new Modules[] { Modules.REDE }, Permissoes = new Permissions[] { Permissions.CONSULTAR })]
        public ActionResult GetVaosDemandasByCidade(long idCidade)
        {
            IEnumerable<VaosDemandaPoste> vaos_demandas_bd = UnitOfWork.VaosDemandaPosteRepository.Get(
                p => p.IdCidade == idCidade && p.DataExclusao == null, includeProperties: "Cidade");

            if (vaos_demandas_bd != null && vaos_demandas_bd.Count() > 0)
                return Json(new
                {
                    VaosDemandas = VaosDemandasPaginadoView.GenerateByOs(vaos_demandas_bd)
                    //Limites = new LimitesCidadeView().LimitesByOS(UnitOfWork.OrdemDeServicoRepository.Get(o => o.NumeroOS == codOs, includeProperties: "Cidade,PoligonosOS").FirstOrDefault())
                }, JsonRequestBehavior.AllowGet);
            else
                return Json(new { Msg = Resources.Messages.Not_Register_Data_Base }, JsonRequestBehavior.AllowGet);
        }

        //public ActionResult SalvarVaos(VaosDemandasView vaosDemandasView)
        //{
        //    Cidade cidade = UnitOfWork.CidadeRepository.Get(v => v.IdCidade == vaosDemandasView.IdCidade).FirstOrDefault();
        //    if(cidade != null){
        //        OrdemDeServico ordem = UnitOfWork.OrdemDeServicoRepository.Get(or => or.NumeroOS == vaosDemandasView.NumeroOs).FirstOrDefault();

        //        VaosDemandaPoste VaosDemandaPoste = new VaosDemandaPoste
        //        {
        //            IdPontoEntrega = 
        //        };
        //    }
        //}
    }
}