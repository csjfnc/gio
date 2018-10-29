using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Website.BLL.Entities;
using Website.BLL.Utils.Geocoding;

namespace Website.MVC.Models.Maps
{
    public class StrandPaginadoView
    {
        public static object Generate(IEnumerable strands)
        {
            if (strands == null)
                throw new ArgumentException("o paramêtro 'strands' não pode ser null.");

            long IdCidade = -1;
            ConverterUtmToLatLon converter = null;
            List<object> StrandList = new List<object>();
            LatLon centro;
            double x = 0, y = 0, z = 0;
            int total = 0;
            string NomeCidade = string.Empty;

            foreach (DemandaStrand poste in strands)
            {
                if (converter == null) converter = new ConverterUtmToLatLon(poste.Cidade.Datum, poste.Cidade.NorteOuSul, poste.Cidade.Zona);
                if (IdCidade == -1) IdCidade = poste.IdCidade;
                if (NomeCidade == string.Empty) NomeCidade = poste.Cidade.Nome;

                LatLon LatiLong1 = converter.Convert(poste.X1, poste.Y1);
                LatLon LatiLong2 = converter.Convert(poste.X2, poste.Y2);

                //Calculando o Centro do conjunto dos postes
                double latitude = LatiLong1.Lat * Math.PI / 180;
                double longitude = LatiLong1.Lon * Math.PI / 180;

                x += Math.Cos(latitude) * Math.Cos(longitude);
                y += Math.Cos(latitude) * Math.Sin(longitude);
                z += Math.Sin(latitude);
                total++;

                StrandList.Add(new DemandaStrandView
                {
                    IdDemandaStrand = poste.ID,
                    X1 = LatiLong1.Lat,
                    Y1 = LatiLong1.Lon,
                    X2 = LatiLong2.Lat,
                    Y2 = LatiLong2.Lon
                });
            }

            x = x / total;
            y = y / total;
            z = z / total;

            double centralLongitude = Math.Atan2(y, x);
            double centralSquareRoot = Math.Sqrt(x * x + y * y);
            double centralLatitude = Math.Atan2(z, centralSquareRoot);

            centro = new LatLon() { Lat = centralLatitude * 180 / Math.PI, Lon = centralLongitude * 180 / Math.PI };

            return new { IdCidade = IdCidade, NomeCidade = NomeCidade, DemandaStrands = StrandList, Centro = centro };
        }


        public static object GenerateUnico(DemandaStrand strands)
        {
            if (strands == null)
                throw new ArgumentException("o paramêtro 'strands' não pode ser null.");

            long IdCidade = -1;
            ConverterUtmToLatLon converter = null;
            List<object> StrandList = new List<object>();
          //  LatLon centro;
          //  double x = 0, y = 0, z = 0;
          //  int total = 0;
            string NomeCidade = string.Empty;


            if (converter == null) converter = new ConverterUtmToLatLon(strands.Cidade.Datum, strands.Cidade.NorteOuSul, strands.Cidade.Zona);
            if (IdCidade == -1) IdCidade = strands.IdCidade;
            if (NomeCidade == string.Empty) NomeCidade = strands.Cidade.Nome;

            LatLon LatiLong1 = converter.Convert(strands.X1, strands.Y1);
            LatLon LatiLong2 = converter.Convert(strands.X2, strands.Y2);

                ////Calculando o Centro do conjunto dos postes
                //double latitude = LatiLong1.Lat * Math.PI / 180;
                //double longitude = LatiLong1.Lon * Math.PI / 180;

                //x += Math.Cos(latitude) * Math.Cos(longitude);
                //y += Math.Cos(latitude) * Math.Sin(longitude);
                //z += Math.Sin(latitude);
                //total++;

                DemandaStrandView DemandaStrandView = new DemandaStrandView
                {
                    ID = strands.ID,
                    X1 = LatiLong1.Lat,
                    Y1 = LatiLong1.Lon,
                    X2 = LatiLong2.Lat,
                    Y2 = LatiLong2.Lon
                };            

            //x = x / total;
            //y = y / total;
            //z = z / total;

            //double centralLongitude = Math.Atan2(y, x);
            //double centralSquareRoot = Math.Sqrt(x * x + y * y);
            //double centralLatitude = Math.Atan2(z, centralSquareRoot);

            //centro = new LatLon() { Lat = centralLatitude * 180 / Math.PI, Lon = centralLongitude * 180 / Math.PI };

            return new {DemandaStrand = DemandaStrandView};
        }
    }
}