using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Website.BLL.Entities;
using Website.BLL.Utils.Geocoding;
using Website.DAL.UnitOfWork;
using Website.MVC.Models.Maps;

namespace Website.MVC.Controllers
{
    public class QuadraController : Controller
    {
        private readonly UnitOfWork UnitOfWork = new UnitOfWork();

        public ActionResult GetQuadrasByOs(string OrdemServico)
        {
            OrdemDeServico os = UnitOfWork.OrdemDeServicoRepository.Get(o => o.NumeroOS == OrdemServico, includeProperties: "Cidade").FirstOrDefault();

            List<Quadra> quadras = UnitOfWork.QuadrasRepository.Get(q => q.IdOrdemDeServico == os.IdOrdemDeServico).ToList();

            List<object> quadrasRetorno = new List<object>();

            ConverterUtmToLatLon converter = new ConverterUtmToLatLon(os.Cidade.Datum, os.Cidade.NorteOuSul, os.Cidade.Zona);

            foreach (Quadra quadraBD in quadras)
            {
                LatLon LatiLongA = converter.Convert(quadraBD.X1, quadraBD.Y1);
                LatLon LatiLongB = converter.Convert(quadraBD.X2, quadraBD.Y2);

                quadrasRetorno.Add(new 
                {
                    ID = quadraBD.ID,
                    LatitudeA = LatiLongA.Lat,
                    LongitudeA = LatiLongA.Lon,
                    LatitudeB = LatiLongB.Lat,
                    LongitudeB = LatiLongB.Lon,
                });
            }

            return Json(new ResponseView() { Status = Status.OK, Result = quadrasRetorno }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetQuadrasByCidade(long IdCidade)
        {
            Cidade cidade = UnitOfWork.CidadeRepository.Get(c => c.IdCidade == IdCidade).FirstOrDefault();

            List<Quadra> quadras = UnitOfWork.QuadrasRepository.Get(q => q.IdCidade == IdCidade).ToList();

            List<object> quadrasRetorno = new List<object>();

            ConverterUtmToLatLon converter = new ConverterUtmToLatLon(cidade.Datum, cidade.NorteOuSul, cidade.Zona);

            foreach (Quadra quadraBD in quadras)
            {
                LatLon LatiLongA = converter.Convert(quadraBD.X1, quadraBD.Y1);
                LatLon LatiLongB = converter.Convert(quadraBD.X2, quadraBD.Y2);

                quadrasRetorno.Add(new
                {
                    ID = quadraBD.ID,
                    LatitudeA = LatiLongA.Lat,
                    LongitudeA = LatiLongA.Lon,
                    LatitudeB = LatiLongB.Lat,
                    LongitudeB = LatiLongB.Lon,
                });
            }

            return Json(new ResponseView() { Status = Status.OK, Result = quadrasRetorno }, JsonRequestBehavior.AllowGet);
        }
	}
}