using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Website.BLL.Entities;
using Website.BLL.Utils.Geocoding;
using Website.DAL.UnitOfWork;
using Website.MVC.Util; 

namespace Website.MVC.Controllers
{
    public class LocalUserWebController : Controller
    {
        private readonly UnitOfWork UnitOfWork = new UnitOfWork();

        [HttpGet]
        public ActionResult PegarLocalUser()
        {
            
            List<Object> LocalUsuarioAtualLista = new List<Object>();
            string data = "";

            foreach (var item in Centralizador.IdUser_Position)
            {                
                data = string.Format("{0:dd/MM/yyyy HH:mm}", item.Value.Time);

                Object localUsuarioAtual = new
                {
                    NomeUser = item.Key.ToUpper(),
                    X = item.Value.LatLon.Lat,
                    Y = item.Value.LatLon.Lon,
                    Time = data,
                    Panico = item.Value.Panico,
                    Mensagem = item.Value.Mensagem
                };                    
                LocalUsuarioAtualLista.Add(localUsuarioAtual);
            }
            if (LocalUsuarioAtualLista.Count <= 0)
            {                
                List<LocalUsuarioAtual> LocalUsuarioAtualLista_bd = UnitOfWork.LocalUsuarioAtualRepository.Get().ToList();
                foreach (var item in LocalUsuarioAtualLista_bd)
                {
                    LatLon latLon = new LatLon
                    {
                        Lat = item.X,
                        Lon = item.Y
                    };

                    LatLonTime latLonTime = new LatLonTime
                    {
                        LatLon = latLon,
                        Time = item.TimeBanco,
                        Panico = item.Panico,
                        Mensagem = item.Mensagem
                    };

                    if (Centralizador.IdUser_Position.ContainsKey(item.NomeUser.ToUpper()))
                    {
                        Centralizador.IdUser_Position[item.NomeUser.ToUpper()] = latLonTime;
                    }
                    else
                    {
                        Centralizador.IdUser_Position.Add(item.NomeUser.ToUpper(), latLonTime);
                    }                     

                    data = string.Format("{0:dd/MM/yyyy HH:mm}", item.TimeBanco);
                    Object localUsuarioAtual = new
                        {
                            NomeUser = item.NomeUser.ToUpper(),
                            X = item.X,
                            Y = item.Y,
                            Time = data,
                            Panico = item.Panico,
                            Mensagem = item.Mensagem
                        };                    
                    LocalUsuarioAtualLista.Add(localUsuarioAtual);
                }
            }
            return Json(new {LocaisUsers =  LocalUsuarioAtualLista }, JsonRequestBehavior.AllowGet);
        }

	}
}