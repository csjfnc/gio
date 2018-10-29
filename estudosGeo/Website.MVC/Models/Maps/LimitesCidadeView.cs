using System.Collections.Generic;
using Website.BLL.Entities;
using Website.BLL.Utils.Geocoding;

namespace Website.MVC.Models.Maps
{
    public class LimitesCidadeView
    {
        public double LatitudeA { get; set; }
        public double LongitudeA { get; set; }
        public double LatitudeB { get; set; }
        public double LongitudeB { get; set; }

        public List<LimitesCidadeView> LimitesByCidade(List<LimiteCidade> Limites) 
        {
            List<LimitesCidadeView> retorno = new List<LimitesCidadeView>();

            if (Limites != null && Limites.Count > 0) 
            {
                foreach (LimiteCidade Limite in Limites)
                {
                    ConverterUtmToLatLon converter = new ConverterUtmToLatLon(Limite.Cidade.Datum, Limite.Cidade.NorteOuSul, Limite.Cidade.Zona);

                    retorno.Add(new LimitesCidadeView
                        {
                            LatitudeA = converter.Convert(Limite.X1, Limite.Y1).Lat,
                            LongitudeA = converter.Convert(Limite.X1, Limite.Y1).Lon,
                            LatitudeB = converter.Convert(Limite.X2, Limite.Y2).Lat,
                            LongitudeB = converter.Convert(Limite.X2, Limite.Y2).Lon,
                        });
                }
            }

            return retorno;
        }

        public List<LimitesCidadeView> LimitesByOS(OrdemDeServico ordem_servico)
        {
            List<LimitesCidadeView> retorno = new List<LimitesCidadeView>();

            if (ordem_servico != null) 
            {
                if (ordem_servico.PoligonosOS.Count > 0)
                {
                    foreach (PoligonoOS poligono in ordem_servico.PoligonosOS)
                    {
                        ConverterUtmToLatLon converter = new ConverterUtmToLatLon(ordem_servico.Cidade.Datum, ordem_servico.Cidade.NorteOuSul, ordem_servico.Cidade.Zona);

                        retorno.Add(new LimitesCidadeView
                        {
                            LatitudeA = converter.Convert(poligono.X1, poligono.Y1).Lat,
                            LongitudeA = converter.Convert(poligono.X1, poligono.Y1).Lon,
                            LatitudeB = converter.Convert(poligono.X2, poligono.Y2).Lat,
                            LongitudeB = converter.Convert(poligono.X2, poligono.Y2).Lon,
                        });
                    }
                }
            }

            return retorno;
        }
    }
}