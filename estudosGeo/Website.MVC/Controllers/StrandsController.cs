using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Website.BLL.Entities;
using Website.BLL.Utils.Geocoding;
using Website.DAL.UnitOfWork;
using Website.MVC.Helpers.CustomController;
using Website.MVC.Models.Maps;

namespace Website.MVC.Controllers
{
    public class StrandsController : BaseController
    {

        private readonly UnitOfWork UnitOfWork = new UnitOfWork();
        long IdCidade = -1;
        ConverterUtmToLatLon converter = null;
        string NomeCidade = string.Empty;

        //
        // GET: /Strands/
        public ActionResult GetStrandsByOs(string codOs)
        {
            var ordemServico = UnitOfWork.OrdemDeServicoRepository.Get(nu => nu.NumeroOS == codOs).FirstOrDefault();

            List<DemandaStrand> DemandaStrands = new List<DemandaStrand>();
            var listaDemandaStrand = UnitOfWork.DemandaStrandRepository.Get(a => a.IdOrdemDeServico == ordemServico.IdOrdemDeServico && a.DataExclusao == null, includeProperties: "OrdemDeServico, Cidade").ToList();

            foreach (var item in listaDemandaStrand)
            {

                if (converter == null) converter = new ConverterUtmToLatLon(item.OrdemDeServico.Cidade.Datum, item.OrdemDeServico.Cidade.NorteOuSul, item.OrdemDeServico.Cidade.Zona);
                if (IdCidade == -1) IdCidade = item.OrdemDeServico.IdCidade;
                if (NomeCidade == string.Empty) NomeCidade = item.OrdemDeServico.Cidade.Nome;

                LatLon LatiLong1 = converter.Convert(item.X1, item.Y1);
                LatLon LatiLong2 = converter.Convert(item.X2, item.Y2);
                DemandaStrands.Add(new DemandaStrand()
                {
                    ID = item.ID,
                    X1 = LatiLong1.Lat,
                    Y1 = LatiLong1.Lon, 
                    X2 = LatiLong2.Lat,
                    Y2 = LatiLong2.Lon,
                });
            }
            return Json(new ResponseView() { Status = Status.OK, Result = DemandaStrands }, JsonRequestBehavior.AllowGet);
        }

        [HttpDelete]
        public ActionResult ExcluirStrands(long id)
        {
            DemandaStrand demandaStrand = UnitOfWork.DemandaStrandRepository.Get(s => s.ID == id).FirstOrDefault();
            if (demandaStrand != null)
            {
                demandaStrand.DataExclusao = DateTime.Now;
                UnitOfWork.DemandaStrandRepository.Update(demandaStrand);
                UnitOfWork.Save();
                return Json(new ResponseView() { Status = Status.OK, Result = "OK" }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(new ResponseView() { Status = Status.NotFound, Result = "Erro" }, JsonRequestBehavior.AllowGet);
            }
            
        }

        [HttpPost]
        public ActionResult Novo(DemandaStrandView demandaStrandView)
        {
            Cidade cidade = UnitOfWork.CidadeRepository.Get(c => c.IdCidade == demandaStrandView.IdCidade).FirstOrDefault();
            ConverterLatLonToUtm converter = null;
            if(cidade != null){
                converter = new ConverterLatLonToUtm(cidade.Datum, cidade.NorteOuSul, cidade.Zona);

                Poste poste1 = UnitOfWork.PosteRepository.Get(p1 => p1.IdPoste == demandaStrandView.IdPoste1).FirstOrDefault();
                Poste poste2 = UnitOfWork.PosteRepository.Get(p2 => p2.IdPoste == demandaStrandView.IdPoste2).FirstOrDefault();

                OrdemDeServico ordemDeServico = UnitOfWork.OrdemDeServicoRepository.Get(or => or.NumeroOS == demandaStrandView.NumeroOs).FirstOrDefault();

               /* UTM utm1 = converter.Convert(demandaStrandView.X1, demandaStrandView.Y1);
                UTM utm2 = converter.Convert(demandaStrandView.X2, demandaStrandView.Y2);*/

                DemandaStrand demandaStrand = new DemandaStrand
                {
                    IdCidade = demandaStrandView.IdCidade,
                    IdOrdemDeServico = ordemDeServico.IdOrdemDeServico,
                    X1 = poste1.X,
                    Y1 = poste1.Y,
                    X2 = poste2.X,
                    Y2 = poste2.Y,
                    DataInclusao = DateTime.Now
                };

                UnitOfWork.DemandaStrandRepository.Insert(demandaStrand);
                UnitOfWork.Save();

                DemandaStrand strand = UnitOfWork.DemandaStrandRepository.Get(d => d.ID == demandaStrand.ID).FirstOrDefault();
                                
                return Json( StrandPaginadoView.GenerateUnico(strand), JsonRequestBehavior.AllowGet);

            }
            else
            {
                return Json(new {Status = Status.NotFound, Result = "Cidade nao encontrada!" }, JsonRequestBehavior.AllowGet);
            }
        }
        [HttpPost]
        public ActionResult NovoNoMaps([System.Web.Http.FromBody] DemandaStrandView demandaStrandView)
        {
            Cidade cidade = UnitOfWork.CidadeRepository.Get(c => c.IdCidade == demandaStrandView.IdCidade).FirstOrDefault();
            ConverterLatLonToUtm converter = null;
            if (cidade != null)
            {
                converter = new ConverterLatLonToUtm(cidade.Datum, cidade.NorteOuSul, cidade.Zona);
                

                OrdemDeServico ordemDeServico = UnitOfWork.OrdemDeServicoRepository.Get(or => or.NumeroOS == demandaStrandView.NumeroOs).FirstOrDefault();

                double x1 = Double.Parse(demandaStrandView.X1Texto, System.Globalization.CultureInfo.InvariantCulture);
                double y1 = Double.Parse(demandaStrandView.Y1Texto, System.Globalization.CultureInfo.InvariantCulture);
                double x2 = Double.Parse(demandaStrandView.X2Texto, System.Globalization.CultureInfo.InvariantCulture);
                double y2 = Double.Parse(demandaStrandView.Y2Texto, System.Globalization.CultureInfo.InvariantCulture);

                 UTM utm1 = converter.Convert(x1, y1);
                 UTM utm2 = converter.Convert(x2, y2);

                DemandaStrand demandaStrand = new DemandaStrand
                {
                    IdCidade = demandaStrandView.IdCidade,
                    IdOrdemDeServico = ordemDeServico.IdOrdemDeServico,
                    X1 = utm1.X,
                    Y1 = utm1.Y,
                    X2 = utm2.X,
                    Y2 = utm2.Y,
                    DataInclusao = DateTime.Now
                };

                UnitOfWork.DemandaStrandRepository.Insert(demandaStrand);
                UnitOfWork.Save();

                DemandaStrand strand = UnitOfWork.DemandaStrandRepository.Get(d => d.ID == demandaStrand.ID).FirstOrDefault();

                return Json(StrandPaginadoView.GenerateUnico(strand), JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(new { Status = Status.NotFound, Result = "Cidade nao encontrada!" }, JsonRequestBehavior.AllowGet);
            }
        }
	}
}
 