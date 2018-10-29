using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Website.BLL.Entities;
using Website.BLL.Utils.Geocoding;
using Website.DAL.UnitOfWork;

namespace Website.MVC.Models.Maps
{
    public class VaosDemandasPaginadoView
    {

        private readonly UnitOfWork UnitOfWork = new UnitOfWork();

        public static object GenerateByOs(IEnumerable<VaosDemandaPoste> postesOs)
        {
            if (postesOs == null)
                throw new ArgumentException("o paramêtro 'postesOs' não pode ser null.");

            long IdCidade = -1;
            ConverterUtmToLatLon converter = null;
            List<object> vaosdemandas = new List<object>();
            // LatLon centro;
            // double x = 0, y = 0, z = 0;
            // int total = 0;
             string NomeCidade = string.Empty;

            foreach (VaosDemandaPoste vao in postesOs)
            {
                if (converter == null) converter = new ConverterUtmToLatLon(vao.Cidade.Datum, vao.Cidade.NorteOuSul, vao.Cidade.Zona);
                if (IdCidade == -1) IdCidade = vao.IdCidade;
                if (NomeCidade == string.Empty) NomeCidade = vao.Cidade.Nome;

                LatLon LatiLong1 = converter.Convert(vao.X1, vao.Y1);
                LatLon LatiLong2 = converter.Convert(vao.X2, vao.Y2);

                //Calculando o Centro do conjunto dos postes
                /*double latitude = LatiLong.Lat * Math.PI / 180;
                double longitude = LatiLong.Lon * Math.PI / 180;*/

                /*x += Math.Cos(latitude) * Math.Cos(longitude);
                y += Math.Cos(latitude) * Math.Sin(longitude);
                z += Math.Sin(latitude);
                total++; */

                vaosdemandas.Add(new VaosDemandasView
                {
                    ID = vao.IdPontoEntrega,
                    X1 = LatiLong1.Lat,
                    Y1 = LatiLong1.Lon,
                    X2 = LatiLong2.Lat,
                    Y2 = LatiLong2.Lon

                    /*IdPontoEntrega = poste.IdPontoEntrega,
                    Latitude = LatiLong.Lat,
                    Longitude = LatiLong.Lon,
                    //CodGeo = poste.CodigoGeo,
                    //NumeroPosteNaOS = poste.NumeroPosteNaOS,
                    Img = ImgAux*/
                });
            }

           /* x = x / total;
            y = y / total;
            z = z / total;

            double centralLongitude = Math.Atan2(y, x);
            double centralSquareRoot = Math.Sqrt(x * x + y * y);
            double centralLatitude = Math.Atan2(z, centralSquareRoot);*/

           // centro = new LatLon() { Lat = centralLatitude * 180 / Math.PI, Lon = centralLongitude * 180 / Math.PI };

            return new { VaosDemandas = vaosdemandas };
        }

        public VaosDemandasView VaoToVaoView(VaosDemandaPoste vaos_bd)
        {
            //Poste poste = UnitOfWork.PosteRepository.Get(p => p.IdPoste == vaos_bd.IdPoste).FirstOrDefault();
            Cidade cidade = UnitOfWork.CidadeRepository.Get(c => c.IdCidade == vaos_bd.IdCidade).FirstOrDefault();

            ConverterUtmToLatLon converter = new ConverterUtmToLatLon(cidade.Datum, cidade.NorteOuSul, cidade.Zona);
            LatLon LatiLong1 = converter.Convert(vaos_bd.X1, vaos_bd.Y1);
            LatLon LatiLong2 = converter.Convert(vaos_bd.X2, vaos_bd.Y2);

            VaosDemandasView vaos = new VaosDemandasView();

            vaos.X1 = LatiLong1.Lat;
            vaos.Y1 = LatiLong1.Lon;
            vaos.X2 = LatiLong2.Lat;
            vaos.Y2 = LatiLong2.Lon;

            return vaos;
        }
    }
}