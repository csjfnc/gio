using System;
using System.Linq;
using System.Web.Mvc;
using Website.BLL.Entities;
using Website.DAL.UnitOfWork;
using Website.Identity.CustomAutorizes;
using Website.MVC.Helpers.CustomAttribute;
using Website.MVC.Models;

namespace Website.MVC.Controllers
{
    public class CelularController : Controller
    {
        private readonly UnitOfWorkMobile UnitOfWork = new UnitOfWorkMobile();

        protected override void Dispose(bool disposing)
        {
            UnitOfWork.Dispose();
            base.Dispose(disposing);
        }
        
        [AppAuthorize(Modulos = new Modules[] { Modules.MOBILE })]
        public ActionResult Index()
        {
            return View(UnitOfWork.Repository.GetAll().ToList());
        }

        [AppAuthorize(Modulos = new Modules[] { Modules.MOBILE })]
        public ActionResult AddCelular()
        {
            return View();
        }

        [HttpPost]
        [AppAuthorize(Modulos = new Modules[] { Modules.MOBILE })]
        public ActionResult AddCelular(CelularViewModel Celular)
        {
            if (ModelState.IsValid)
            {
                Mobile mobile = new Mobile()
                {
                    Imei = Celular.Imei,
                    IsBlock = Celular.IsBlock,
                    Name = Celular.Name,
                    PhoneNumber = Celular.PhoneNumber,
                    ExpiresOn = DateTime.Now,
                    IssuedOn = DateTime.Now
                };

                UnitOfWork.Repository.Insert(mobile);
                Celular.Id = mobile.Id;

                TempData["MSG"] = "O Celular com o Imei: " + Celular.Imei + " foi cadastrado com Sucesso.";
                return RedirectToAction("Index", "Celular");
                                               
            }

            return View(Celular);
        }

        [AppAuthorize(Modulos = new Modules[] { Modules.MOBILE })]
        public ActionResult Excluir(long IdMobile)
        {
            UnitOfWork.Repository.Delete(IdMobile);
            TempData["MSG"] = Resources.Messages.Save_OK;

            return RedirectToAction("Index", "Celular");
        }

        [AppAuthorize(Modulos = new Modules[] { Modules.MOBILE })]
        public ActionResult EditCelular(long idMobile)
        {
            Mobile mobile = UnitOfWork.Repository.GetById(idMobile);
            if (mobile != null)
            {
                CelularViewModel celularViewMovel = new CelularViewModel()
                {
                    Id = mobile.Id,
                    Imei = mobile.Imei,
                    IsBlock = mobile.IsBlock,
                    Name = mobile.Name,
                    PhoneNumber = mobile.PhoneNumber                    
                };

                return View(celularViewMovel);
            }
            else
            {
                TempData["MSG"] = Resources.Messages.Not_Register_Data_Base;
                return RedirectToAction("Index", "Celular");
            }
        }

        [HttpPost]
        [AppAuthorize(Modulos = new Modules[] { Modules.MOBILE })]
        public ActionResult EditCelular(CelularViewModel celular)
        {
            if (ModelState.IsValid)
            {
                Mobile mobile = UnitOfWork.Repository.GetById(celular.Id);
                if (mobile != null)
                {
                    mobile.Imei = celular.Imei;
                    mobile.Name = celular.Name;
                    mobile.PhoneNumber = celular.PhoneNumber;
                    mobile.IsBlock = celular.IsBlock;

                    UnitOfWork.Repository.Update(mobile);

                    TempData["MSG"] = Resources.Messages.Save_OK;
                    return RedirectToAction("Index", "Celular");
                }
                else
                {
                    TempData["MSG"] = Resources.Messages.Not_Register_Data_Base;
                    return RedirectToAction("Index", "Celular");
                }
            }

            return View(celular);
        }
    }
}