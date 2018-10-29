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
    public class FotoArvoreController : Controller
    {
        private readonly UnitOfWork UnitOfWork = new UnitOfWork();

        [HttpGet]
        public ActionResult GetFotosByArvore(long IdArvore)
        {
            List<FotoArvore> fotos = UnitOfWork.FotoArvoreRepository.Get(f => f.IdArvore == IdArvore && f.DataExclusao == null).ToList();

            return Json(fotos, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public void GetFoto(long IdFoto, int Width, int Height)
        {
            FotoArvore foto = UnitOfWork.FotoArvoreRepository.Get(f => f.IdFotoArvore == IdFoto).FirstOrDefault();
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