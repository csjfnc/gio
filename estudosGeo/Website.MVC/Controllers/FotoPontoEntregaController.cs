using System.Web.Mvc;
using Website.DAL.UnitOfWork;
using System.Web.Helpers;
using System.Linq;
using Website.BLL.Entities;
using System.Collections.Generic;
using Website.MVC.Models.Maps;

namespace Website.MVC.Controllers
{
    [Authorize]
    public class FotoPontoEntregaController : Controller
    {
        private readonly UnitOfWork UnitOfWork = new UnitOfWork();

        [HttpGet]
        public ActionResult GetFotosByPontoEntrega(long IdPontoEntrega)
        {
            List<FotoPontoEntrega> fotos = UnitOfWork.FotoPontoEntregaRepository.Get(f => f.IdPontoEntrega == IdPontoEntrega && f.DataExclusao == null).ToList();
            List<FotoPontoEntregaView> fotos_view = new List<FotoPontoEntregaView>();

            foreach (FotoPontoEntrega foto_bd in fotos)
            {
                fotos_view.Add(new FotoPontoEntregaView () { IdFotoPontoEntrega = foto_bd.IdFotoPontoEntrega, DataFoto = string.Format("{0:yyyy-MM-dd}", foto_bd.DataFoto), NumeroFoto = foto_bd.NumeroFoto });
            }

            return Json(fotos_view, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public void GetFoto(long IdFoto, int Width, int Height)
        {
            FotoPontoEntrega foto = UnitOfWork.FotoPontoEntregaRepository.Get(f => f.IdFotoPontoEntrega == IdFoto).FirstOrDefault();
            if (foto != null && System.IO.File.Exists(foto.Path))
            {
                new WebImage(foto.Path).Resize(Width, Height).Write();
            }
            else
            {
                new WebImage("~/Images/no-image-available.png").Resize(Width, Height).Write();
            }
        }

        [HttpGet]
        public void SemFoto()
        {
            new WebImage("~/Images/SemFoto.jpg").Resize(500, 400).Write();
        }
    }
}