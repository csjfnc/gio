using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Web;
using Website.BLL.Entities;
using Website.BLL.Utils.Geocoding;
using Website.DAL.UnitOfWork;

namespace Website.MVC.Util
{
    public static class Centralizador
    {
        private static readonly UnitOfWork UnitOfWork = new UnitOfWork();
        public static bool GerenciadorGravaLocalUserbanco = false;

        public static IDictionary<string, LatLonTime> IdUser_Position = new Dictionary<string, LatLonTime>();

        internal static void GravaLocalUserbanco()
        {
            if (!GerenciadorGravaLocalUserbanco)
            {
                GerenciadorGravaLocalUserbanco = true;
                Thread GravaLocalUserbanco = new Thread(() =>
                {
                    while (true)
                    {
                        Thread.Sleep(3*(1000*60));                        
                        //Thread.Sleep(7000);                        

                        lock (IdUser_Position)
                        {
                            gravar();
                        }
                    }
                });
                GravaLocalUserbanco.Start();
            }
        }

        public static void gravar()
        {
            foreach (var item in IdUser_Position)
            {
                LocalUsuarioAtual LocalUsuarioAtual_bd = UnitOfWork.LocalUsuarioAtualRepository.Get(lo => lo.NomeUser == item.Key.ToUpper()).FirstOrDefault();

                if (LocalUsuarioAtual_bd != null)
                {
                    LocalUsuarioAtual_bd.X = item.Value.LatLon.Lat;
                    LocalUsuarioAtual_bd.Y = item.Value.LatLon.Lon;
                    LocalUsuarioAtual_bd.TimeBanco = item.Value.Time;
                    LocalUsuarioAtual_bd.Panico = item.Value.Panico;
                    LocalUsuarioAtual_bd.Mensagem = item.Value.Mensagem;
                    UnitOfWork.Save();
                }
                else
                {
                LocalUsuarioAtual localUsuarioAtual = new LocalUsuarioAtual
                   {
                       NomeUser = item.Key.ToUpper(),
                       X = item.Value.LatLon.Lat,
                       Y = item.Value.LatLon.Lon,
                       TimeBanco = item.Value.Time,
                       Panico = item.Value.Panico,
                       Mensagem = item.Value.Mensagem
                   };
                   UnitOfWork.LocalUsuarioAtualRepository.Insert(localUsuarioAtual);
                   UnitOfWork.Save();
                }
                //   data = string.Format("{0:dd/MM/yyyy HH:mm}", item.Value.Time);

                /*   LocalUsuarioAtual localUsuarioAtual = new LocalUsuarioAtual
                   {
                       NomeUser = item.Key,
                       X = item.Value.LatLon.Lat,
                       Y = item.Value.LatLon.Lon,
                       TimeBanco = item.Value.Time
                   };
                   UnitOfWork.LocalUsuarioAtualRepository.Insert(localUsuarioAtual);
                   UnitOfWork.Save();*/
            }
        }
    }
}