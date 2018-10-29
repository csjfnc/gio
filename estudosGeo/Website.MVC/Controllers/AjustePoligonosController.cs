using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Website.BLL.Entities;
using Website.BLL.Utils.Validacoes;
using Website.DAL.UnitOfWork;
using Website.Identity.CustomAutorizes;
using Website.MVC.Helpers.CustomAttribute;

namespace Website.MVC.Controllers
{
    public class AjustePoligonosController : Controller
    {
        private readonly UnitOfWork UnitOfWork = new UnitOfWork();

        [AppAuthorize(Modulos = new Modules[] { Modules.ADMINISTRACAO })]
        public ActionResult Index()
        {
            ViewBag.IdCidade = new SelectList(UnitOfWork.CidadeRepository.Get().ToList(), "IdCidade", "Nome", "0");
            return View(UnitOfWork.OrdemDeServicoRepository.Get(includeProperties: "Cidade").ToList());
        }

        [HttpPost]
        [AppAuthorize(Modulos = new Modules[] { Modules.ADMINISTRACAO })]
        public ActionResult Index(int id)
        {
            OrdemDeServico os = UnitOfWork.OrdemDeServicoRepository.Get(x => x.IdOrdemDeServico == id, includeProperties: "PoligonosOS").FirstOrDefault();
            if (os != null)
            {
                foreach (PoligonoOS polig in os.PoligonosOS.OrderPolygon())
                {
                    UnitOfWork.PoligonoOSRepository.Update(polig);
                }

                UnitOfWork.Save();

                ViewBag.Msg = "Ajuste da Ordem de Serviço: " + os.NumeroOS + " realizada com sucesso.";
            }
            else
            {
                ViewBag.Error = "Não foi encontrado a ordem de servico no banco de dados.";
            }

            ViewBag.IdCidade = new SelectList(UnitOfWork.CidadeRepository.Get().ToList(), "IdCidade", "Nome", "0");
            return View(UnitOfWork.OrdemDeServicoRepository.Get(includeProperties: "Cidade").ToList());
        }

        [HttpPost]
        [AppAuthorize(Modulos = new Modules[] { Modules.ADMINISTRACAO })]
        public ActionResult AjustAll()
        {
            List<OrdemDeServico> ordensDeServico = UnitOfWork.OrdemDeServicoRepository.Get(includeProperties: "PoligonosOS").ToList();
            foreach (OrdemDeServico os in ordensDeServico)
            {
                foreach (PoligonoOS polig in os.PoligonosOS.OrderPolygon())
                {
                    UnitOfWork.PoligonoOSRepository.Update(polig);
                }
                UnitOfWork.Save();
            }

            ViewBag.msg = "Ajuste de todas as Ordens de Serviço realizada com sucesso.";

            return View();
        }

        [HttpPost]
        [AppAuthorize(Modulos = new Modules[] { Modules.ADMINISTRACAO })]
        public ActionResult AjustByCidade(string IdCidade)
        {
            long Id = long.Parse(IdCidade);
            Cidade Cidade = UnitOfWork.CidadeRepository.Get(c => c.IdCidade == Id).FirstOrDefault();
            List<OrdemDeServico> os = UnitOfWork.OrdemDeServicoRepository.Get(o => o.IdCidade == Id, includeProperties: "PoligonosOS,Cidade").ToList();
            if (os != null)
            {
                foreach (OrdemDeServico ordem in os)
                {
                    foreach (PoligonoOS polig in ordem.PoligonosOS.OrderPolygon())
                    {
                        UnitOfWork.PoligonoOSRepository.Update(polig);
                    }
                    UnitOfWork.Save();
                }

                ViewBag.msg = "Ajuste da Cidade com o Cidade: " + Cidade.Nome + " realizada com sucesso.";
            }
            else
            {
                ViewBag.Error = "Não foi encontrado a ordem de servico no banco de dados com o Cidade: " + Cidade.Nome + ".";
            }

            return View();
        }
    }
}