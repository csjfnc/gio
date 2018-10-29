using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Website.BLL.Entities;
using Website.BLL.Utils.Geocoding;

namespace Website.MVC.Models.Maps
{
    public class AnotacaoPaginadoView
    {
        public static object Generate(IEnumerable anotacaoes)
        {
            if (anotacaoes == null)
                throw new ArgumentException("o paramêtro 'strands' não pode ser null.");

            long IdCidade = -1;
            ConverterUtmToLatLon converter = null;
            List<object> AnotacaoList = new List<object>();
            LatLon centro;
            double x = 0, y = 0, z = 0;
            int total = 0;
            string NomeCidade = string.Empty;

            foreach (Anotacao poste in anotacaoes)
            {
                if (converter == null) converter = new ConverterUtmToLatLon(poste.Cidade.Datum, poste.Cidade.NorteOuSul, poste.Cidade.Zona);
                if (IdCidade == -1) IdCidade = poste.IdCidade;
                if (NomeCidade == string.Empty) NomeCidade = poste.Cidade.Nome;

                LatLon LatiLong1 = converter.Convert(poste.X, poste.Y); 

                //Calculando o Centro do conjunto dos postes
                double latitude = LatiLong1.Lat * Math.PI / 180;
                double longitude = LatiLong1.Lon * Math.PI / 180;

                x += Math.Cos(latitude) * Math.Cos(longitude);
                y += Math.Cos(latitude) * Math.Sin(longitude);
                z += Math.Sin(latitude);
                total++;

                AnotacaoList.Add(new AnotacaoView
                {
                    IdAnotacao = poste.IdAnotacao,
                    X = LatiLong1.Lat,
                    Y = LatiLong1.Lon, 
                    Descricao = poste.Descricao
                });
            }

            x = x / total;
            y = y / total;
            z = z / total;

            double centralLongitude = Math.Atan2(y, x);
            double centralSquareRoot = Math.Sqrt(x * x + y * y);
            double centralLatitude = Math.Atan2(z, centralSquareRoot);

            centro = new LatLon() { Lat = centralLatitude * 180 / Math.PI, Lon = centralLongitude * 180 / Math.PI };

            return new { IdCidade = IdCidade, NomeCidade = NomeCidade, Anotacao = AnotacaoList, Centro = centro };
        }

        public AnotacaoView AnotacaToAnotacaoView(Anotacao anotacao)
        {
            if (anotacao == null)
                throw new ArgumentException("o paramêtro 'Poste' não pode ser null.");

            ConverterUtmToLatLon converter = new ConverterUtmToLatLon(anotacao.Cidade.Datum, anotacao.Cidade.NorteOuSul, anotacao.Cidade.Zona);
            LatLon LatiLong = converter.Convert(anotacao.X, anotacao.Y);
            AnotacaoView anotacaoView = new AnotacaoView();

            anotacaoView.IdAnotacao = anotacao.IdAnotacao;
            anotacaoView.Descricao = anotacao.Descricao;
            anotacaoView.X = LatiLong.Lat;
            anotacaoView.Y = LatiLong.Lon;
            anotacaoView.IdCidade = anotacao.IdCidade;            

            return anotacaoView;
        }

        public PosteView NovoPoste(Poste poste)
        {

            ConverterUtmToLatLon converter = new ConverterUtmToLatLon(poste.Cidade.Datum, poste.Cidade.NorteOuSul, poste.Cidade.Zona);
            LatLon LatiLong = converter.Convert(poste.X, poste.Y);

            return new PosteView { IdPoste = poste.IdPoste, Latitude = LatiLong.Lat, Longitude = LatiLong.Lon, CodGeo = poste.CodigoGeo };

        }
    }
}