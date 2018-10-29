using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Website.BLL.Enums;

namespace Website.MVC.Models.Maps
{
    public class PontoEntregaView
    {
        public long IdPontoEntrega { get; set; }
        public long IdPoste { get; set; }
        public long CodigoGeoBD { get; set; }
        public long? IdOrdemServico { get; set; }
        public string IdOrdemServicoTexto { get; set; }
        public long IdCidade { get; set; }
      
        public string Numero { get; set; }
        public ClasseSocial ClasseSocial { get; set; }
        public string Ocorrencia { get; set; }
        public long IdLogradouro { get; set; }

        public double Latitude { get; set; }
        public double Longitude { get; set; }


        public string LatitudeTexto { get; set; }
        public string LongitudeTexto { get; set; }
        

        public List<FotoPontoEntregaView> Fotos { get; set; }

        public string Classificacao { get; set; }
        public string Complemento1 { get; set; }
        public string Complemento2 { get; set; }
        public string TipoImovel { get; set; }
        public string QtdDomicilio { get; set; }
        public string QtdDomicilioComercio { get; set; }
        public int NumeroAndaresEdificio { get; set; }
        public int TotalApartamentosEdificio { get; set; }
        public string NomeEdificio { get; set; }
        public string QtdBlocos { get; set; }
        public string Img { get; set; }
        public string ClassificacaoComercio { get; set; }


        public string LatitudePosteTexto { get; set; }
        public string LongitudePosteTexto { get; set; }






        //public string Fase { get; set; }
        //public string EtLigacao { get; set; }
        //public string Observacao { get; set; }
        //public StatusEquipamento Status { get; set; }
        // public ClasseAtendimento ClasseAtendimento { get; set; }
        // public TipoConstrucao TipoConstrucao { get; set; }       
        //public int QuantidadeMedidores { get; set; }
        //  public List<MedidorView> Medidores { get; set; }

    }
}