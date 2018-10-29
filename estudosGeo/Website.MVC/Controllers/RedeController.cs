using System.Web.Mvc;
using Website.DAL.UnitOfWork;
using Website.Identity.CustomAutorizes;
using Website.MVC.Helpers.CustomAttribute;
using Website.MVC.Models.Maps;
using System.Linq;

namespace Website.MVC.Controllers
{
    public class RedeController : Controller
    {
        private readonly UnitOfWork UnitOfWork = new UnitOfWork();

        [AppAuthorize(Modulos = new Modules[] { Modules.REDE }, Permissoes = new Permissions[] { Permissions.CONSULTAR })]
        public ActionResult Index()
        {
            return View(new CidadeViewModel() { Cidades = new SelectList(UnitOfWork.CidadeRepository.Get(orderBy: q => q.OrderBy(d => d.Nome)).ToList(), "IdCidade", "Nome") });
        }

        [Authorize]
        public ActionResult StreetView() 
        {
            return View();
        }

        protected override void Dispose(bool disposing)
        {
            UnitOfWork.Dispose();
            base.Dispose(disposing);
        }
    }
}