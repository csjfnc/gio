using System;
using System.Collections;
using System.Collections.Generic;
using Website.BLL.Entities;
using Website.BLL.Utils.Geocoding;

namespace Website.MVC.Models.Maps
{
    public class PostePaginadoView
    {
        public static object Generate(IEnumerable postesCurr)
        {
            if (postesCurr == null)
                throw new ArgumentException("o paramêtro 'PostesCurr' não pode ser null.");

            long IdCidade = -1;
            ConverterUtmToLatLon converter = null;
            List<object> postes = new List<object>();
            LatLon centro;
            double x = 0, y = 0, z = 0;
            int total = 0;
            string NomeCidade = string.Empty;

            foreach (Poste poste in postesCurr)
            {
                if (converter == null)
                    converter = new ConverterUtmToLatLon(poste.Cidade.Datum, poste.Cidade.NorteOuSul, poste.Cidade.Zona);

                if (IdCidade == -1)
                    IdCidade = poste.Cidade.IdCidade;

                if (NomeCidade == string.Empty)
                    NomeCidade = poste.Cidade.Nome;

                LatLon LatiLong = converter.Convert(poste.X, poste.Y);

                //Calculando o Centro do conjunto dos postes
                double latitude = LatiLong.Lat * Math.PI / 180;
                double longitude = LatiLong.Lon * Math.PI / 180;

                x += Math.Cos(latitude) * Math.Cos(longitude);
                y += Math.Cos(latitude) * Math.Sin(longitude);
                z += Math.Sin(latitude);
                total++;

                // Nova regra de Status do poste para a View
                string ImgAux = string.Empty;
                if (poste.TipoPoste == BLL.Enums.TipoPoste.ARVORE_G || poste.TipoPoste == BLL.Enums.TipoPoste.ARVORE_M || poste.TipoPoste == BLL.Enums.TipoPoste.ARVORE_P)
                {
                    ImgAux = "tree";
                }
                else if (poste.TipoPoste == BLL.Enums.TipoPoste.INVASAO_RISCO_01 || poste.TipoPoste == BLL.Enums.TipoPoste.INVASAO_RISCO_02)
                {
                    ImgAux = "exclamacao";
                }
                else if (poste.CodigoGeo <= 0 || poste.CodigoGeo >= 800000000)//else if (poste.CodigoGeo <= 0)
                {
                    ImgAux = "05";
                }
                else if (poste.DataExclusao != null)
                {
                    ImgAux = "02";
                }
                else if (poste.Finalizado)
                {
                    //ImgAux = "03";
                    ImgAux = "10";
                }
                else
                {
                    ImgAux = "08";
                }

                postes.Add(new PosteView
                {
                    IdPoste = poste.IdPoste,
                    Latitude = LatiLong.Lat,
                    Longitude = LatiLong.Lon,
                    CodGeo = poste.CodigoGeo,
                    NumeroPosteNaOS = poste.NumeroPosteNaOS,
                    Img = ImgAux,
                    Finalizado = poste.Finalizado
                    
                });
            }

            x = x / total;
            y = y / total;
            z = z / total;

            double centralLongitude = Math.Atan2(y, x);
            double centralSquareRoot = Math.Sqrt(x * x + y * y);
            double centralLatitude = Math.Atan2(z, centralSquareRoot);

            centro = new LatLon() { Lat = centralLatitude * 180 / Math.PI, Lon = centralLongitude * 180 / Math.PI };

            return new { IdCidade = IdCidade, NomeCidade = NomeCidade, Postes = postes, Centro = centro };
        }

        public static object GenerateByOs(IEnumerable<Poste> postesOs)
        {
            if (postesOs == null)
                throw new ArgumentException("o paramêtro 'postesOs' não pode ser null.");

            long IdCidade = -1;
            ConverterUtmToLatLon converter = null;
            List<object> postes = new List<object>();
            LatLon centro;
            double x = 0, y = 0, z = 0;
            int total = 0;
            string NomeCidade = string.Empty;

            foreach (Poste poste in postesOs)
            {
                if (converter == null) converter = new ConverterUtmToLatLon(poste.Cidade.Datum, poste.Cidade.NorteOuSul, poste.Cidade.Zona);
                if (IdCidade == -1) IdCidade = poste.IdCidade;
                if (NomeCidade == string.Empty) NomeCidade = poste.Cidade.Nome;

                LatLon LatiLong = converter.Convert(poste.X, poste.Y);

                //Calculando o Centro do conjunto dos postes
                double latitude = LatiLong.Lat * Math.PI / 180;
                double longitude = LatiLong.Lon * Math.PI / 180;

                x += Math.Cos(latitude) * Math.Cos(longitude);
                y += Math.Cos(latitude) * Math.Sin(longitude);
                z += Math.Sin(latitude);
                total++;

                // Nova regra de Status do poste para a View
                string ImgAux = string.Empty;
                if (poste.TipoPoste == BLL.Enums.TipoPoste.ARVORE_G || poste.TipoPoste == BLL.Enums.TipoPoste.ARVORE_M || poste.TipoPoste == BLL.Enums.TipoPoste.ARVORE_P)
                {
                    ImgAux = "tree";
                }
                else if (poste.TipoPoste == BLL.Enums.TipoPoste.INVASAO_RISCO_01 || poste.TipoPoste == BLL.Enums.TipoPoste.INVASAO_RISCO_02)
                {
                    ImgAux = "07";
                }
                else if (poste.CodigoGeo <= 0)
                {
                    ImgAux = "05";
                }
                else if (poste.DataExclusao != null)
                {
                    ImgAux = "02";
                }
                else if (poste.Finalizado)
                {
                    //ImgAux = "03";
                    ImgAux = "10";
                }
                else
                {
                    ImgAux = "08";
                }

                postes.Add(new PosteView
                {
                    IdPoste = poste.IdPoste,
                    Latitude = LatiLong.Lat,
                    Longitude = LatiLong.Lon,
                    CodGeo = poste.CodigoGeo,
                    NumeroPosteNaOS = poste.NumeroPosteNaOS,
                    Img = ImgAux,
                    Finalizado = poste.Finalizado
                });
            }

            x = x / total;
            y = y / total;
            z = z / total;

            double centralLongitude = Math.Atan2(y, x);
            double centralSquareRoot = Math.Sqrt(x * x + y * y);
            double centralLatitude = Math.Atan2(z, centralSquareRoot);

            centro = new LatLon() { Lat = centralLatitude * 180 / Math.PI, Lon = centralLongitude * 180 / Math.PI };

            return new { IdCidade = IdCidade, NomeCidade = NomeCidade, Postes = postes, Centro = centro };
        }

        #region Filtro Potencia

        public static object GeneratePotencia(IEnumerable postesCurr)
        {
            if (postesCurr == null)
                throw new ArgumentException("o paramêtro 'PostesCurr' não pode ser null.");

            long IdCidade = -1;
            ConverterUtmToLatLon converter = null;
            List<object> postes = new List<object>();
            LatLon centro;
            double x = 0, y = 0, z = 0;
            int total = 0;
            string NomeCidade = string.Empty;

            foreach (Poste poste in postesCurr)
            {
                if (converter == null) converter = new ConverterUtmToLatLon(poste.Cidade.Datum, poste.Cidade.NorteOuSul, poste.Cidade.Zona);
                if (IdCidade == -1) IdCidade = poste.IdCidade;
                if (NomeCidade == string.Empty) NomeCidade = poste.Cidade.Nome;

                LatLon LatiLong = converter.Convert(poste.X, poste.Y);

                //Calculando o Centro do conjunto dos postes
                double latitude = LatiLong.Lat * Math.PI / 180;
                double longitude = LatiLong.Lon * Math.PI / 180;

                x += Math.Cos(latitude) * Math.Cos(longitude);
                y += Math.Cos(latitude) * Math.Sin(longitude);
                z += Math.Sin(latitude);
                total++;

                postes.Add(new PosteView
                {
                    IdPoste = poste.IdPoste,
                    Latitude = LatiLong.Lat,
                    Longitude = LatiLong.Lon,
                    CodGeo = poste.CodigoGeo,
                    Img = new MetodosPosteView().IdentificaCorPostePotencia(poste)
                });
            }

            x = x / total;
            y = y / total;
            z = z / total;

            double centralLongitude = Math.Atan2(y, x);
            double centralSquareRoot = Math.Sqrt(x * x + y * y);
            double centralLatitude = Math.Atan2(z, centralSquareRoot);

            centro = new LatLon() { Lat = centralLatitude * 180 / Math.PI, Lon = centralLongitude * 180 / Math.PI };

            return new { IdCidade = IdCidade, NomeCidade = NomeCidade, Postes = postes, Centro = centro };
        }

        public static object GenerateByOsPotencia(IEnumerable<Poste> postesOs)
        {
            if (postesOs == null)
                throw new ArgumentException("o paramêtro 'postesOs' não pode ser null.");

            long IdCidade = -1;
            ConverterUtmToLatLon converter = null;
            List<object> postes = new List<object>();
            LatLon centro;
            double x = 0, y = 0, z = 0;
            int total = 0;
            string NomeCidade = string.Empty;

            foreach (Poste poste in postesOs)
            {
                if (converter == null) converter = new ConverterUtmToLatLon(poste.Cidade.Datum, poste.Cidade.NorteOuSul, poste.Cidade.Zona);
                if (IdCidade == -1) IdCidade = poste.IdCidade;
                if (NomeCidade == string.Empty) NomeCidade = poste.Cidade.Nome;

                LatLon LatiLong = converter.Convert(poste.X, poste.Y);

                //Calculando o Centro do conjunto dos postes
                double latitude = LatiLong.Lat * Math.PI / 180;
                double longitude = LatiLong.Lon * Math.PI / 180;

                x += Math.Cos(latitude) * Math.Cos(longitude);
                y += Math.Cos(latitude) * Math.Sin(longitude);
                z += Math.Sin(latitude);
                total++;

                postes.Add(new PosteView
                {
                    IdPoste = poste.IdPoste,
                    Latitude = LatiLong.Lat,
                    Longitude = LatiLong.Lon,
                    CodGeo = poste.CodigoGeo,
                    Img = new MetodosPosteView().IdentificaCorPostePotencia(poste)
                });
            }

            x = x / total;
            y = y / total;
            z = z / total;

            double centralLongitude = Math.Atan2(y, x);
            double centralSquareRoot = Math.Sqrt(x * x + y * y);
            double centralLatitude = Math.Atan2(z, centralSquareRoot);

            centro = new LatLon() { Lat = centralLatitude * 180 / Math.PI, Lon = centralLongitude * 180 / Math.PI };

            return new { IdCidade = IdCidade, NomeCidade = NomeCidade, Postes = postes, Centro = centro };
        }
        
        #endregion

        #region Filtro Lampada

        public static object GenerateLampada(IEnumerable postesCurr)
        {
            if (postesCurr == null)
                throw new ArgumentException("o paramêtro 'PostesCurr' não pode ser null.");

            long IdCidade = -1;
            ConverterUtmToLatLon converter = null;
            List<object> postes = new List<object>();
            LatLon centro;
            double x = 0, y = 0, z = 0;
            int total = 0;
            string NomeCidade = string.Empty;

            foreach (Poste poste in postesCurr)
            {
                if (converter == null) converter = new ConverterUtmToLatLon(poste.Cidade.Datum, poste.Cidade.NorteOuSul, poste.Cidade.Zona);
                if (IdCidade == -1) IdCidade = poste.IdCidade;
                if (NomeCidade == string.Empty) NomeCidade = poste.Cidade.Nome;

                LatLon LatiLong = converter.Convert(poste.X, poste.Y);

                //Calculando o Centro do conjunto dos postes
                double latitude = LatiLong.Lat * Math.PI / 180;
                double longitude = LatiLong.Lon * Math.PI / 180;

                x += Math.Cos(latitude) * Math.Cos(longitude);
                y += Math.Cos(latitude) * Math.Sin(longitude);
                z += Math.Sin(latitude);
                total++;

                postes.Add(new PosteView
                {
                    IdPoste = poste.IdPoste,
                    Latitude = LatiLong.Lat,
                    Longitude = LatiLong.Lon,
                    CodGeo = poste.CodigoGeo,
                    Img = new MetodosPosteView().IdentificaCorPosteLampada(poste)
                });
            }

            x = x / total;
            y = y / total;
            z = z / total;

            double centralLongitude = Math.Atan2(y, x);
            double centralSquareRoot = Math.Sqrt(x * x + y * y);
            double centralLatitude = Math.Atan2(z, centralSquareRoot);

            centro = new LatLon() { Lat = centralLatitude * 180 / Math.PI, Lon = centralLongitude * 180 / Math.PI };

            return new { IdCidade = IdCidade, NomeCidade = NomeCidade, Postes = postes, Centro = centro };
        }

        public static object GenerateByOsLampada(IEnumerable<Poste> postesOs)
        {
            if (postesOs == null)
                throw new ArgumentException("o paramêtro 'postesOs' não pode ser null.");

            long IdCidade = -1;
            ConverterUtmToLatLon converter = null;
            List<object> postes = new List<object>();
            LatLon centro;
            double x = 0, y = 0, z = 0;
            int total = 0;
            string NomeCidade = string.Empty;

            foreach (Poste poste in postesOs)
            {
                if (converter == null) converter = new ConverterUtmToLatLon(poste.Cidade.Datum, poste.Cidade.NorteOuSul, poste.Cidade.Zona);
                if (IdCidade == -1) IdCidade = poste.IdCidade;
                if (NomeCidade == string.Empty) NomeCidade = poste.Cidade.Nome;

                LatLon LatiLong = converter.Convert(poste.X, poste.Y);

                //Calculando o Centro do conjunto dos postes
                double latitude = LatiLong.Lat * Math.PI / 180;
                double longitude = LatiLong.Lon * Math.PI / 180;

                x += Math.Cos(latitude) * Math.Cos(longitude);
                y += Math.Cos(latitude) * Math.Sin(longitude);
                z += Math.Sin(latitude);
                total++;

                postes.Add(new PosteView
                {
                    IdPoste = poste.IdPoste,
                    Latitude = LatiLong.Lat,
                    Longitude = LatiLong.Lon,
                    CodGeo = poste.CodigoGeo,
                    Img = new MetodosPosteView().IdentificaCorPosteLampada(poste)
                });
            }

            x = x / total;
            y = y / total;
            z = z / total;

            double centralLongitude = Math.Atan2(y, x);
            double centralSquareRoot = Math.Sqrt(x * x + y * y);
            double centralLatitude = Math.Atan2(z, centralSquareRoot);

            centro = new LatLon() { Lat = centralLatitude * 180 / Math.PI, Lon = centralLongitude * 180 / Math.PI };

            return new { IdCidade = IdCidade, NomeCidade = NomeCidade, Postes = postes, Centro = centro };
        }

        #endregion

        #region Filtro Status

        public static object GenerateStatus(IEnumerable postesCurr)
        {
            if (postesCurr == null)
                throw new ArgumentException("o paramêtro 'PostesCurr' não pode ser null.");

            long IdCidade = -1;
            ConverterUtmToLatLon converter = null;
            List<object> postes = new List<object>();
            LatLon centro;
            double x = 0, y = 0, z = 0;
            int total = 0;
            string NomeCidade = string.Empty;

            foreach (Poste poste in postesCurr)
            {
                if (converter == null) converter = new ConverterUtmToLatLon(poste.Cidade.Datum, poste.Cidade.NorteOuSul, poste.Cidade.Zona);
                if (IdCidade == -1) IdCidade = poste.IdCidade;
                if (NomeCidade == string.Empty) NomeCidade = poste.Cidade.Nome;

                LatLon LatiLong = converter.Convert(poste.X, poste.Y);

                //Calculando o Centro do conjunto dos postes
                double latitude = LatiLong.Lat * Math.PI / 180;
                double longitude = LatiLong.Lon * Math.PI / 180;

                x += Math.Cos(latitude) * Math.Cos(longitude);
                y += Math.Cos(latitude) * Math.Sin(longitude);
                z += Math.Sin(latitude);
                total++;

                postes.Add(new PosteView
                {
                    IdPoste = poste.IdPoste,
                    Latitude = LatiLong.Lat,
                    Longitude = LatiLong.Lon,
                    CodGeo = poste.CodigoGeo,
                 //   Img = new MetodosPosteView().IdentificaCorPosteStatusIp(poste.IP)
                });
            }

            x = x / total;
            y = y / total;
            z = z / total;

            double centralLongitude = Math.Atan2(y, x);
            double centralSquareRoot = Math.Sqrt(x * x + y * y);
            double centralLatitude = Math.Atan2(z, centralSquareRoot);

            centro = new LatLon() { Lat = centralLatitude * 180 / Math.PI, Lon = centralLongitude * 180 / Math.PI };

            return new { IdCidade = IdCidade, NomeCidade = NomeCidade, Postes = postes, Centro = centro };
        }

        public static object GenerateByOsStatus(IEnumerable<Poste> postesOs)
        {
            if (postesOs == null)
                throw new ArgumentException("o paramêtro 'postesOs' não pode ser null.");

            long IdCidade = -1;
            ConverterUtmToLatLon converter = null;
            List<object> postes = new List<object>();
            LatLon centro;
            double x = 0, y = 0, z = 0;
            int total = 0;
            string NomeCidade = string.Empty;

            foreach (Poste poste in postesOs)
            {
                if (converter == null) converter = new ConverterUtmToLatLon(poste.Cidade.Datum, poste.Cidade.NorteOuSul, poste.Cidade.Zona);
                if (IdCidade == -1) IdCidade = poste.IdCidade;
                if (NomeCidade == string.Empty) NomeCidade = poste.Cidade.Nome;

                LatLon LatiLong = converter.Convert(poste.X, poste.Y);

                //Calculando o Centro do conjunto dos postes
                double latitude = LatiLong.Lat * Math.PI / 180;
                double longitude = LatiLong.Lon * Math.PI / 180;

                x += Math.Cos(latitude) * Math.Cos(longitude);
                y += Math.Cos(latitude) * Math.Sin(longitude);
                z += Math.Sin(latitude);
                total++;

                postes.Add(new PosteView
                {
                    IdPoste = poste.IdPoste,
                    Latitude = LatiLong.Lat,
                    Longitude = LatiLong.Lon,
                    CodGeo = poste.CodigoGeo,
           //         Img = new MetodosPosteView().IdentificaCorPosteStatusIp(poste.IP)
                });
            }

            x = x / total;
            y = y / total;
            z = z / total;

            double centralLongitude = Math.Atan2(y, x);
            double centralSquareRoot = Math.Sqrt(x * x + y * y);
            double centralLatitude = Math.Atan2(z, centralSquareRoot);

            centro = new LatLon() { Lat = centralLatitude * 180 / Math.PI, Lon = centralLongitude * 180 / Math.PI };

            return new { IdCidade = IdCidade, NomeCidade = NomeCidade, Postes = postes, Centro = centro };
        }

        #endregion

        #region Filtro NA SA

        public static object GenerateNaSa(IEnumerable postesCurr)
        {
            if (postesCurr == null)
                throw new ArgumentException("o paramêtro 'PostesCurr' não pode ser null.");

            long IdCidade = -1;
            ConverterUtmToLatLon converter = null;
            List<object> postes = new List<object>();
            LatLon centro;
            double x = 0, y = 0, z = 0;
            int total = 0;
            string NomeCidade = string.Empty;

            foreach (Poste poste in postesCurr)
            {
                if (converter == null) converter = new ConverterUtmToLatLon(poste.Cidade.Datum, poste.Cidade.NorteOuSul, poste.Cidade.Zona);
                if (IdCidade == -1) IdCidade = poste.IdCidade;
                if (NomeCidade == string.Empty) NomeCidade = poste.Cidade.Nome;

                LatLon LatiLong = converter.Convert(poste.X, poste.Y);

                //Calculando o Centro do conjunto dos postes
                double latitude = LatiLong.Lat * Math.PI / 180;
                double longitude = LatiLong.Lon * Math.PI / 180;

                x += Math.Cos(latitude) * Math.Cos(longitude);
                y += Math.Cos(latitude) * Math.Sin(longitude);
                z += Math.Sin(latitude);
                total++;

                postes.Add(new PosteView
                {
                    IdPoste = poste.IdPoste,
                    Latitude = LatiLong.Lat,
                    Longitude = LatiLong.Lon,
                    CodGeo = poste.CodigoGeo,
                    Img = new MetodosPosteView().IdentificaCorPosteNaSa(poste)
                });
            }

            x = x / total;
            y = y / total;
            z = z / total;

            double centralLongitude = Math.Atan2(y, x);
            double centralSquareRoot = Math.Sqrt(x * x + y * y);
            double centralLatitude = Math.Atan2(z, centralSquareRoot);

            centro = new LatLon() { Lat = centralLatitude * 180 / Math.PI, Lon = centralLongitude * 180 / Math.PI };

            return new { IdCidade = IdCidade, NomeCidade = NomeCidade, Postes = postes, Centro = centro };
        }

        public static object GenerateByOsNaSa(IEnumerable<Poste> postesOs)
        {
            if (postesOs == null)
                throw new ArgumentException("o paramêtro 'postesOs' não pode ser null.");

            long IdCidade = -1;
            ConverterUtmToLatLon converter = null;
            List<object> postes = new List<object>();
            LatLon centro;
            double x = 0, y = 0, z = 0;
            int total = 0;
            string NomeCidade = string.Empty;

            foreach (Poste poste in postesOs)
            {
                if (converter == null) converter = new ConverterUtmToLatLon(poste.Cidade.Datum, poste.Cidade.NorteOuSul, poste.Cidade.Zona);
                if (IdCidade == -1) IdCidade = poste.IdCidade;
                if (NomeCidade == string.Empty) NomeCidade = poste.Cidade.Nome;

                LatLon LatiLong = converter.Convert(poste.X, poste.Y);

                //Calculando o Centro do conjunto dos postes
                double latitude = LatiLong.Lat * Math.PI / 180;
                double longitude = LatiLong.Lon * Math.PI / 180;

                x += Math.Cos(latitude) * Math.Cos(longitude);
                y += Math.Cos(latitude) * Math.Sin(longitude);
                z += Math.Sin(latitude);
                total++;

                postes.Add(new PosteView
                {
                    IdPoste = poste.IdPoste,
                    Latitude = LatiLong.Lat,
                    Longitude = LatiLong.Lon,
                    CodGeo = poste.CodigoGeo,
                    Img = new MetodosPosteView().IdentificaCorPosteNaSa(poste)
                });
            }

            x = x / total;
            y = y / total;
            z = z / total;

            double centralLongitude = Math.Atan2(y, x);
            double centralSquareRoot = Math.Sqrt(x * x + y * y);
            double centralLatitude = Math.Atan2(z, centralSquareRoot);

            centro = new LatLon() { Lat = centralLatitude * 180 / Math.PI, Lon = centralLongitude * 180 / Math.PI };

            return new { IdCidade = IdCidade, NomeCidade = NomeCidade, Postes = postes, Centro = centro };
        }

        #endregion

        public static object GeneratePosteUnico(Poste poste)
        {
            if (poste == null)
                throw new ArgumentException("o paramêtro 'postesOs' não pode ser null.");

            long IdCidade = -1;
            ConverterUtmToLatLon converter = null;
            List<object> postes = new List<object>();
            LatLon centro;
            double x = 0, y = 0, z = 0;
            int total = 0;
            string NomeCidade = string.Empty;

                if (converter == null) converter = new ConverterUtmToLatLon(poste.Cidade.Datum, poste.Cidade.NorteOuSul, poste.Cidade.Zona);
                if (IdCidade == -1) IdCidade = poste.IdCidade;
                if (NomeCidade == string.Empty) NomeCidade = poste.Cidade.Nome;

                LatLon LatiLong = converter.Convert(poste.X, poste.Y);

                //Calculando o Centro do conjunto dos postes
                double latitude = LatiLong.Lat * Math.PI / 180;
                double longitude = LatiLong.Lon * Math.PI / 180;

                x += Math.Cos(latitude) * Math.Cos(longitude);
                y += Math.Cos(latitude) * Math.Sin(longitude);
                z += Math.Sin(latitude);
                total++;

                // Nova regra de Status do poste para a View
                string ImgAux = string.Empty;
                if (poste.TipoPoste == BLL.Enums.TipoPoste.ARVORE_G || poste.TipoPoste == BLL.Enums.TipoPoste.ARVORE_M || poste.TipoPoste == BLL.Enums.TipoPoste.ARVORE_P)
                {
                    ImgAux = "tree";
                }
                else if (poste.TipoPoste == BLL.Enums.TipoPoste.INVASAO_RISCO_01 || poste.TipoPoste == BLL.Enums.TipoPoste.INVASAO_RISCO_02)
                {
                    ImgAux = "07";
                }
                else if (poste.CodigoGeo <= 0)
                {
                    ImgAux = "05";
                }
                else if (poste.DataExclusao != null)
                {
                    ImgAux = "02";
                }
                else if (poste.Finalizado)
                {
                    //ImgAux = "03";
                    ImgAux = "10";
                }
                else
                {
                    ImgAux = "08";
                }

                PosteView posteView = new PosteView
                {
                    IdPoste = poste.IdPoste,
                    Latitude = LatiLong.Lat,
                    Longitude = LatiLong.Lon,
                    CodGeo = poste.CodigoGeo,
                    NumeroPosteNaOS = poste.NumeroPosteNaOS,
                    Img = ImgAux
                };
            

            //x = x / total;
            //y = y / total;
            //z = z / total;

            //double centralLongitude = Math.Atan2(y, x);
            //double centralSquareRoot = Math.Sqrt(x * x + y * y);
            //double centralLatitude = Math.Atan2(z, centralSquareRoot);

            //centro = new LatLon() { Lat = centralLatitude * 180 / Math.PI, Lon = centralLongitude * 180 / Math.PI };

            return posteView;
        }
    }
}