using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Website.BLL.Entities;
using Website.BLL.Enums;
using Website.BLL.Utils.Geocoding;
using Website.DAL.UnitOfWork;

namespace Website.MVC.Models.Maps
{
    public class MetodosPontoEntregaView
    {
        private readonly UnitOfWork UnitOfWork = new UnitOfWork();

        /// <summary>
        /// Lista De Pontos de Entrega View para o java script 
        /// </summary>
        /// <param name="pontos_bd"></param>
        /// <returns></returns>
        public List<PontoEntregaView> ListaPontoEntrega(List<PontoEntrega> pontos_bd, long IdPoste)
        {
            List<PontoEntregaView> pontos_entrega = new List<PontoEntregaView>();

            Poste poste = UnitOfWork.PosteRepository.Get(p => p.IdPoste == IdPoste).FirstOrDefault();
            Cidade cidade = UnitOfWork.CidadeRepository.Get(c => c.IdCidade == poste.IdCidade).FirstOrDefault();

            foreach (PontoEntrega ponto_corrente in pontos_bd)
            {
                ConverterUtmToLatLon converter = new ConverterUtmToLatLon(cidade.Datum, cidade.NorteOuSul, cidade.Zona);
                LatLon LatiLong = converter.Convert(ponto_corrente.X, ponto_corrente.Y);

                PontoEntregaView ponto_view = new PontoEntregaView();

                ponto_view.IdPontoEntrega = ponto_corrente.IdPontoEntrega;
                ponto_view.IdPoste = ponto_corrente.IdPoste;
                ponto_view.CodigoGeoBD = ponto_corrente.CodigoGeoBD;
                ponto_view.IdOrdemServico = ponto_corrente.IdOrdemDeServico;
                ponto_view.Classificacao = ponto_corrente.Classificacao;
                ponto_view.TipoImovel = ponto_corrente.TipoImovel;
                ponto_view.NumeroAndaresEdificio = ponto_corrente.NumeroAndaresEdificio;
                ponto_view.TotalApartamentosEdificio = ponto_corrente.TotalApartamentosEdificio;
                ponto_view.NomeEdificio = ponto_corrente.NomeEdificio;
               // ponto_view.Status = ponto_corrente.Status;
                //ponto_view.ClasseAtendimento = ponto_corrente.ClasseAtendimento;
                //ponto_view.TipoConstrucao = ponto_corrente.TipoConstrucao;
                ponto_view.Numero = ponto_corrente.Numero;
                ponto_view.ClasseSocial = ponto_corrente.ClasseSocial;
                //ponto_view.Logradouro = ponto_corrente.Logradouro;
                //ponto_view.EtLigacao = ponto_corrente.EtLigacao;
                //ponto_view.Observacao = ponto_corrente.Observacao;
                ponto_view.Latitude = LatiLong.Lat;
                ponto_view.Longitude = LatiLong.Lon;
                //ponto_view.QuantidadeMedidores = ponto_corrente.Medidor.Count;
                //ponto_view.Fase = ponto_corrente.Fase;

                pontos_entrega.Add(ponto_view);

            }

            return pontos_entrega;
        }

        public PontoEntregaView PontoEntregaToPontoEntregaView(PontoEntrega ponto_entrega_bd) 
        {
           // Poste poste = UnitOfWork.PosteRepository.Get(p => p.IdPoste == ponto_entrega_bd.IdPoste).FirstOrDefault();
            Cidade cidade = UnitOfWork.CidadeRepository.Get(c => c.IdCidade == ponto_entrega_bd.IdCidade).FirstOrDefault();

            ConverterUtmToLatLon converter = new ConverterUtmToLatLon(cidade.Datum, cidade.NorteOuSul, cidade.Zona);
            LatLon LatiLong = converter.Convert(ponto_entrega_bd.X, ponto_entrega_bd.Y);

            PontoEntregaView ponto_view = new PontoEntregaView();

            ponto_view.IdPontoEntrega = ponto_entrega_bd.IdPontoEntrega;
            ponto_view.IdPoste = ponto_entrega_bd.IdPoste;
            ponto_view.CodigoGeoBD = ponto_entrega_bd.CodigoGeoBD;
            ponto_view.Complemento1 = ponto_entrega_bd.Complemento1;
            ponto_view.Complemento2 = ponto_entrega_bd.Complemento2;
            //ponto_view.Status = ponto_entrega_bd.Status;
            //ponto_view.ClasseAtendimento = ponto_entrega_bd.ClasseAtendimento;
            //ponto_view.TipoConstrucao = ponto_entrega_bd.TipoConstrucao;
            ponto_view.Numero = ponto_entrega_bd.Numero;
            ponto_view.ClasseSocial = ponto_entrega_bd.ClasseSocial;
            //ponto_view.Logradouro = ponto_entrega_bd.Logradouro;
            //ponto_view.Fase = ponto_entrega_bd.Fase;
            //ponto_view.EtLigacao = ponto_entrega_bd.EtLigacao;
            //ponto_view.Observacao = ponto_entrega_bd.Observacao;
            ponto_view.Latitude = LatiLong.Lat;
            ponto_view.Longitude = LatiLong.Lon;
            ponto_view.Classificacao = ponto_entrega_bd.Classificacao;
            ponto_view.ClassificacaoComercio = ponto_entrega_bd.TipoComercio;
            ponto_view.NomeEdificio = ponto_entrega_bd.NomeEdificio;
            ponto_view.NumeroAndaresEdificio = ponto_entrega_bd.NumeroAndaresEdificio;
            ponto_view.Ocorrencia = ponto_entrega_bd.Ocorrencia;
            ponto_view.QtdBlocos = ponto_entrega_bd.QtdBlocos;
            ponto_view.QtdDomicilio = ponto_entrega_bd.QtdDomicilio;
            ponto_view.QtdDomicilioComercio = ponto_entrega_bd.QtdSalas;
            ponto_view.TotalApartamentosEdificio = ponto_entrega_bd.TotalApartamentosEdificio;

            // Nova regra de Status do poste para a View
            string ImgAux = string.Empty;
            if (ponto_view.ClasseSocial == BLL.Enums.ClasseSocial.COMERCIAL_P || ponto_view.ClasseSocial == BLL.Enums.ClasseSocial.COMERCIAL_M || ponto_view.ClasseSocial == BLL.Enums.ClasseSocial.COMERCIAL_G)
            {
                ImgAux = "comercio";
            }
            else if (ponto_view.ClasseSocial == BLL.Enums.ClasseSocial.EDIFÍCIO_RES || ponto_view.ClasseSocial == BLL.Enums.ClasseSocial.EDIFICIO_EMPRESA || ponto_view.ClasseSocial == BLL.Enums.ClasseSocial.EDIFICIO_COM || ponto_view.ClasseSocial == BLL.Enums.ClasseSocial.EDIFÍCIO_CONSTRUCAO)
            {
                ImgAux = "predio";
            }
            else if (ponto_view.ClasseSocial == BLL.Enums.ClasseSocial.RESIDENCIAL)
            {
                ImgAux = "casa";
            }
            else if (ponto_view.ClasseSocial == BLL.Enums.ClasseSocial.TERRENO)
            {
                ImgAux = "terreno";
            }
            else if (ponto_view.ClasseSocial == BLL.Enums.ClasseSocial.COMERCIO_RESIDENCIA)
            {
                ImgAux = "misto";
            }
            else if (ponto_view.ClasseSocial == BLL.Enums.ClasseSocial.VILA)
            {
                ImgAux = "vila";
            }
            else
            {
                ImgAux = "duvida";
            }
            ponto_view.Img = ImgAux;

            return ponto_view;
        }

        public List<MedidorView> MedidoresPontoEntrega(List<Medidor> medidores_bd) 
        {
            List<MedidorView> medidores = new List<MedidorView>();

            foreach (Medidor medidor_corrente in medidores_bd)
            {
                medidores.Add(new MedidorView { 
                    IdMedidor = medidor_corrente.IdMedidor,
                    IdPontoEntrega = medidor_corrente.IdPontoEntrega,
                    NumeroMedidor = medidor_corrente.NumeroMedidor,
                    ComplementoResidencial = medidor_corrente.ComplementoResidencial
                });
            }

            return medidores;
        }

        public List<PontoEntregaMapa> CoordenadasPontoEntrega(long IdCidade) 
        {
            List<PontoEntregaMapa> coordenadas = new List<PontoEntregaMapa>();

            Cidade cidade = UnitOfWork.CidadeRepository.Get(c => c.IdCidade == IdCidade).FirstOrDefault();

            ConverterUtmToLatLon converter = new ConverterUtmToLatLon(cidade.Datum, cidade.NorteOuSul, cidade.Zona);
            
            List<Poste> postes = UnitOfWork.PosteRepository.Get(p => p.IdCidade == IdCidade && p.DataExclusao == null, includeProperties: "PontoDeEntrega").ToList();

            foreach (Poste poste in postes)
            {
                foreach (PontoEntrega ponto_entrega in poste.PontoDeEntrega)
                {
                    LatLon LatLonPoste = converter.Convert(poste.X, poste.Y);
                    LatLon LatLonPontoEntrega = converter.Convert(ponto_entrega.X, ponto_entrega.Y);

                    coordenadas.Add(new PontoEntregaMapa {
                        IdPontoEntrega = ponto_entrega.IdPontoEntrega.ToString(),
                        ponto_entrega_latitude = LatLonPontoEntrega.Lat,
                        ponto_entrega_longitude = LatLonPontoEntrega.Lon,
                        poste_latitude = LatLonPoste.Lat,
                        poste_longitude = LatLonPoste.Lon,
                        Image = ImagePontoEntrega(ponto_entrega.ClasseSocial)
                    }); 
                } 
            }

            return coordenadas;
        }

        internal string ImagePontoEntrega(ClasseSocial classesocial) 
        {
            switch (classesocial)
	        {
                case ClasseSocial.RESIDENCIAL:
                    return "casa.png";
                case ClasseSocial.EDIFÍCIO_RES:
                    return "predio.png";
                case ClasseSocial.COMERCIAL_P:
                    return "comercio.png";
                case ClasseSocial.TERRENO:
                    return "terreno.png";
		        default:
                    return "duvida.png";
	        }
        }

        public List<PontoEntregaMapa> CoordenadasPontoEntregaOrdem(string NumeroOrdem)
        {
            List<PontoEntregaMapa> coordenadas = new List<PontoEntregaMapa>();

            OrdemDeServico ordem = UnitOfWork.OrdemDeServicoRepository.Get(o => o.NumeroOS == NumeroOrdem, includeProperties: "Cidade").FirstOrDefault();

            Cidade cidade = ordem.Cidade;

            ConverterUtmToLatLon converter = new ConverterUtmToLatLon(cidade.Datum, cidade.NorteOuSul, cidade.Zona);

            List<Poste> postes_ordem = UnitOfWork.PosteRepository.Get(p => p.IdOrdemDeServico == ordem.IdOrdemDeServico && p.DataExclusao == null, includeProperties: "PontoDeEntrega").ToList();

            foreach (Poste poste in postes_ordem)
            {
                foreach (PontoEntrega ponto_entrega in poste.PontoDeEntrega)
                {
                    LatLon LatLonPoste = converter.Convert(poste.X, poste.Y);
                    LatLon LatLonPontoEntrega = converter.Convert(ponto_entrega.X, ponto_entrega.Y);

                    coordenadas.Add(new PontoEntregaMapa
                    {
                        IdPontoEntrega = ponto_entrega.IdPontoEntrega.ToString(),
                        ponto_entrega_latitude = LatLonPontoEntrega.Lat,
                        ponto_entrega_longitude = LatLonPontoEntrega.Lon,
                        poste_latitude = LatLonPoste.Lat,
                        poste_longitude = LatLonPoste.Lon,
                        Image = ImagePontoEntrega(ponto_entrega.ClasseSocial)
                    });
                }
            }

            return coordenadas;
        }
    }
}