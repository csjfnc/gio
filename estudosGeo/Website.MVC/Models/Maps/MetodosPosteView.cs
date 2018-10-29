using System;
using System.Collections.Generic;
using Website.BLL.Entities;
using Website.BLL.Utils.Geocoding;

namespace Website.MVC.Models.Maps
{
    public class MetodosPosteView
    {
        public PosteView PostetoPosteView(Poste poste)
        {
            if (poste == null)
                throw new ArgumentException("o paramêtro 'Poste' não pode ser null.");

            ConverterUtmToLatLon converter = new ConverterUtmToLatLon(poste.Cidade.Datum, poste.Cidade.NorteOuSul, poste.Cidade.Zona);
            LatLon LatiLong = converter.Convert(poste.X, poste.Y);
            PosteView posteView = new PosteView();
            posteView.IdPoste = poste.IdPoste;
            posteView.Latitude = LatiLong.Lat;
            posteView.Longitude = LatiLong.Lon;
            posteView.IdCidade = poste.Cidade.IdCidade;
            posteView.CodGeo = poste.CodigoGeo;

            if (poste.Finalizado)
                posteView.Img = "03";
            else
                posteView.Img = "08";
            posteView.Altura = poste.Altura;
            posteView.TipoPoste = poste.TipoPoste;
            posteView.Esforco = poste.Esforco;

            posteView.EncontradoPoste = poste.encontrado;
            posteView.BarramentoPoste = poste.barramento;
            posteView.PararioPoste = poste.para_raio;
            posteView.AterramentoPoste = poste.aterramento;
            posteView.EstruturaPrimariaPoste = poste.estrutura_primaria;
            posteView.EstruturaSecundaria_poste = poste.estrutura_secundaria;

            posteView.QuantidadeEstai = poste.qtd_estai;
            posteView.AnoPoste = poste.ano;
            posteView.SituacaoPoste = poste.situacao;
            posteView.EquipamentoPoste = poste.equipamento1;
            posteView.MuflaPoste = poste.mufla;
            posteView.RedePrimarioPoste = poste.rede_primaria;
            posteView.DefeitoPoste = poste.defeito;            

            /*foreach (var itemIP in poste.IP)
            {
                posteView.NumeroPrefeitura += itemIP.NumeroPrefeitura + " / ";
            }*/

            if (posteView.NumeroPrefeitura != null && posteView.NumeroPrefeitura != string.Empty)
            {
                posteView.NumeroPrefeitura = posteView.NumeroPrefeitura.Remove(posteView.NumeroPrefeitura.Length - 3);

                posteView.Descricao = poste.Descricao + " Numero(s) da Prefeitura: " + posteView.NumeroPrefeitura;
            }
            else
                posteView.Descricao = poste.Descricao;

            return posteView;
        }

        public PosteView NovoPoste(Poste poste)
        {

            ConverterUtmToLatLon converter = new ConverterUtmToLatLon(poste.Cidade.Datum, poste.Cidade.NorteOuSul, poste.Cidade.Zona);
            LatLon LatiLong = converter.Convert(poste.X, poste.Y);

            return new PosteView { IdPoste = poste.IdPoste, Latitude = LatiLong.Lat, Longitude = LatiLong.Lon, CodGeo = poste.CodigoGeo };

        }

        #region Metedos Busca Filtrada Lampada

        public PosteView PostetoPosteViewFiltroLampada(Poste poste)
        {
            if (poste == null)
                throw new ArgumentException("o paramêtro 'Poste' não pode ser null.");

            ConverterUtmToLatLon converter = new ConverterUtmToLatLon(poste.Cidade.Datum, poste.Cidade.NorteOuSul, poste.Cidade.Zona);
            LatLon LatiLong = converter.Convert(poste.X, poste.Y);
            PosteView posteView = new PosteView();
            posteView.IdPoste = poste.IdPoste;
            posteView.Latitude = LatiLong.Lat;
            posteView.Longitude = LatiLong.Lon;
            posteView.Img = IdentificaCorPosteLampada(poste);
            posteView.IdCidade = poste.Cidade.IdCidade;
            posteView.CodGeo = poste.CodigoGeo;

            return posteView;
        }

        public String IdentificaCorPosteLampada(Poste poste)
        {
            string TipoLampada = "";

         /*   foreach (IP ip in poste.IP)
            {
                if(ip.DataExclusao == null)
                    TipoLampada = ip.TipoLampada;
            }*/

            switch (TipoLampada)
            {
                case "SEM IP":
                    return "04";
                case "VS":
                    return "05";
                case "ME":
                    return "06";
                case "VM":
                    return "07";
                case "IN":
                    return "09";
                case "MS":
                    return "10";
                case "FL":
                    return "11";
                default:
                    return "08";
            }
        }

        #endregion

        #region Busca Filtrada Potencia

        public PosteView PostetoPosteViewFiltroPotencia(Poste poste)
        {
            if (poste == null)
                throw new ArgumentException("o paramêtro 'Poste' não pode ser null.");

            ConverterUtmToLatLon converter = new ConverterUtmToLatLon(poste.Cidade.Datum, poste.Cidade.NorteOuSul, poste.Cidade.Zona);
            LatLon LatiLong = converter.Convert(poste.X, poste.Y);
            PosteView posteView = new PosteView();
            posteView.IdPoste = poste.IdPoste;
            posteView.Latitude = LatiLong.Lat;
            posteView.Longitude = LatiLong.Lon;
            posteView.Img = IdentificaCorPostePotencia(poste);
            posteView.IdCidade = poste.Cidade.IdCidade;
            posteView.CodGeo = poste.CodigoGeo;

            return posteView;

        }

        public String IdentificaCorPostePotencia(Poste poste)
        {
            int potencia = -1;

          /*  foreach (IP ip in poste.IP)
            {
                if (ip.DataExclusao == null)
                    potencia = Convert.ToInt32(ip.Potencia);
            }*/

            switch (potencia)
            {
                case 0:
                    return "13";
                case 36:
                    return "14";//novo
                case 40:
                    return "15";
                case 50:
                    return "16";//novo
                case 57:
                    return "17";//novo
                case 58:
                    return "18";
                case 60:
                    return "19";
                case 70:
                    return "20";
                case 71:
                    return "21";//novo
                case 80:
                    return "22";
                case 100:
                    return "23";
                case 125:
                    return "24";
                case 127:
                    return "25";
                case 143:
                    return "26";//novo
                case 150:
                    return "27";
                case 160:
                    return "28";//novo
                case 250:
                    return "29";
                case 400:
                    return "30";
                default:
                    return "12";
            }
        }

        #endregion

        #region Filtro Status 

        public PosteView PostetoPosteViewFiltroStatus(Poste poste) 
        {
            if (poste == null)
                throw new ArgumentException("o paramêtro 'Poste' não pode ser null.");

            ConverterUtmToLatLon converter = new ConverterUtmToLatLon(poste.Cidade.Datum, poste.Cidade.NorteOuSul, poste.Cidade.Zona);
            LatLon LatiLong = converter.Convert(poste.X, poste.Y);
            PosteView posteView = new PosteView();
            posteView.IdPoste = poste.IdPoste;
            posteView.Latitude = LatiLong.Lat;
            posteView.Longitude = LatiLong.Lon;
       //     posteView.Img = IdentificaCorPosteStatusIp(poste.IP);
            posteView.IdCidade = poste.Cidade.IdCidade;
            posteView.CodGeo = poste.CodigoGeo;

            return posteView;
        }

       /* public String IdentificaCorPosteStatus(Poste poste)
        {
            String Status = "";
            //se poste.Status nao nulo Status = poste.Status; se nulo entao Status = "";
            Status = poste.Status != null ? poste.Status : ""; 

            switch (Status)
            {
                case "OK":
                    return "17";
                case "N":
                    return "19";
                case "X":
                    return "22";
                case "A":
                    return "23";
                default:
                    //quando for diferente de OK,N,X,A;
                    return "13";
            }
        }*/

        public String IdentificaCorPosteStatusIp(ICollection<IP> Ips)
        {
            string retorno = "13";

            //Quando OK icon 17.png
            //Quando N icon 19.png
            //Quando X icon 22.png
            //Quando A icon 23.png
            //Icon default 13.png

            if (Ips != null)
            {
                foreach (IP ip_corrente in Ips)
                {
                    if (ip_corrente.StatusComparativo != null) 
                    {
                        if (ip_corrente.StatusComparativo.ToUpper() == "OK")
                        {
                            retorno = "17";
                        }
                        else if (ip_corrente.StatusComparativo.ToUpper() == "N")
                        {
                            //Diferente de seria Ok 
                            if (retorno != "17")
                            {
                                retorno = "19";
                            }
                        }
                        else if (ip_corrente.StatusComparativo.ToUpper() == "X")
                        {
                            //Diferente de seria Ok e Diferente de N
                            if (retorno != "17" && retorno != "19")
                            {
                                retorno = "22";
                            }
                        }
                        else if (ip_corrente.StatusComparativo.ToUpper() == "A")
                        {
                            //Diferente de seria Ok e Diferente de N e Diferente de X
                            if (retorno != "17" && retorno != "19" && retorno != "22")
                            {
                                retorno = "23";
                            }
                        }
                    }
                }
            }

            return retorno;
        }

        #endregion

        public PosteView PostetoPosteViewFiltroNaSa(Poste poste)
        {
            if (poste == null)
                throw new ArgumentException("o paramêtro 'Poste' não pode ser null.");

            ConverterUtmToLatLon converter = new ConverterUtmToLatLon(poste.Cidade.Datum, poste.Cidade.NorteOuSul, poste.Cidade.Zona);
            LatLon LatiLong = converter.Convert(poste.X, poste.Y);
            PosteView posteView = new PosteView();
            posteView.IdPoste = poste.IdPoste;
            posteView.Latitude = LatiLong.Lat;
            posteView.Longitude = LatiLong.Lon;
            posteView.Img = IdentificaCorPosteNaSa(poste);
            posteView.IdCidade = poste.Cidade.IdCidade;
            posteView.CodGeo = poste.CodigoGeo;

            return posteView;
        }

        public String IdentificaCorPosteNaSa(Poste poste)
        {
            String resultado = "";

            foreach (FotoPoste ft in poste.Fotos) 
            {
                if (ft.NumeroFoto == "NA") { resultado = "NA"; }
                else if (ft.NumeroFoto == "SA") { resultado = "SA"; }
            }

            switch (resultado)
            {
                case "NA":
                    return "17";
                case "SA":
                    return "19";
                default:
                    return "13";
            }
        }


    }
}