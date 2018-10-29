using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Website.BLL.Entities;
using Website.BLL.Utils.Geocoding;
using Website.MVC.Util;
using Website.MVC.WebApi.Models; 
using Website.MVC.WebApi.Security;

namespace Website.MVC.WebApi
{
    public class LocalUsuarioController : ApiController
    {
        [HttpPost]
        [LogExceptionMobile]
        public HttpResponseMessage LocalAtual(LocalUsuarioAtual localAtual)
        {
            LatLon lalon = new LatLon{
                Lat = localAtual.X,
                Lon = localAtual.Y
            };
             
            LatLonTime latLonTime = new LatLonTime
            {
                LatLon = lalon,
                Time = DateTime.Now,
                Panico = localAtual.Panico,
                Mensagem = localAtual.Mensagem
            };

            if (Centralizador.IdUser_Position.ContainsKey(localAtual.NomeUser.ToUpper()))
            {
                Centralizador.IdUser_Position[localAtual.NomeUser.ToUpper()] = latLonTime;
            }
            else
            {
                Centralizador.IdUser_Position.Add(localAtual.NomeUser.ToUpper(), latLonTime);
            }

            Centralizador.GravaLocalUserbanco();

            return Request.CreateResponse(HttpStatusCode.OK, new ResponseApi() { Status = Status.OK, Message = Resources.Messages.Save_OK });
        }

        [HttpPost]
        [LogExceptionMobile]
        public HttpResponseMessage Panico(LocalUsuarioAtual UserLocalPanico)
        {
            LatLon lalon = new LatLon
            {
                Lat = UserLocalPanico.X,
                Lon = UserLocalPanico.Y
            };

            LatLonTime latLonTime = new LatLonTime
            {
                Panico = UserLocalPanico.Panico,
                Mensagem = UserLocalPanico.Mensagem,
                LatLon = lalon,
                Time = DateTime.Now
            };

            if (Centralizador.IdUser_Position.ContainsKey(UserLocalPanico.NomeUser.ToUpper()))
            {
                Centralizador.IdUser_Position[UserLocalPanico.NomeUser.ToUpper()] = latLonTime;
            }
            else
            {
                Centralizador.IdUser_Position.Add(UserLocalPanico.NomeUser.ToUpper(), latLonTime);
            }

            return Request.CreateResponse(HttpStatusCode.OK, new ResponseApi() { Status = Status.OK, Message = Resources.Messages.Save_OK });
        }
    }
}
