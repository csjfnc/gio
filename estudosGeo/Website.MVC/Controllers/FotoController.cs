using System.Web.Mvc;
using Website.DAL.UnitOfWork;
using System.Web.Helpers;
using System.Linq;
using Website.BLL.Entities;

namespace Website.MVC.Controllers
{
    [Authorize]
    public class FotosController : Controller
    {
        private readonly UnitOfWork UnitOfWork = new UnitOfWork();

        [HttpGet]
        public void Poste(long IdFoto, int Width, int Height)
        {
            FotoPoste fotos = UnitOfWork.FotoPosteRepository.Get(f => f.IdFotoPoste == IdFoto).FirstOrDefault();
            if (fotos != null && System.IO.File.Exists(fotos.Path))
            {
                new WebImage(fotos.Path).Resize(Width, Height).Write();
            }
            else
            {
                new WebImage("~/Images/no-image-available.png").Resize(Width, Height).Write();
            }
        }

        [HttpGet]
        public void PosteOriginal(long IdFoto)
        {
            FotoPoste fotos = UnitOfWork.FotoPosteRepository.Get(f => f.IdFotoPoste == IdFoto).FirstOrDefault();
            if (fotos != null && System.IO.File.Exists(fotos.Path))
                new WebImage(fotos.Path).Write();
            else
                new WebImage("~/Images/no-image-available.png").Resize(200, 200).Write();
        }

        [HttpGet]
        public void PosteFull(long IdFoto)
        {
            FotoPoste fotos = UnitOfWork.FotoPosteRepository.Get(f => f.IdFotoPoste == IdFoto).FirstOrDefault();
            if (fotos != null && System.IO.File.Exists(fotos.Path))
                new WebImage(fotos.Path).Resize(1024, 1024).Write();
            else
                new WebImage("~/Images/no-image-available.png").Resize(200, 200).Write();
        }

        [HttpGet]
        public void SemFoto()
        {
            new WebImage("~/Images/SemFoto.jpg").Resize(500, 400).Write();
        }

        protected override void Dispose(bool disposing)
        {
            UnitOfWork.Dispose();
            base.Dispose(disposing);
        }
    }
}