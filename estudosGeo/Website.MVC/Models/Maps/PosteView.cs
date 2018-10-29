using System;
using Website.BLL.Enums;
namespace Website.MVC.Models.Maps
{
    public class PosteView
    {
        public long IdPoste { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public string Img { get; set; }
        public long IdCidade { get; set; }
        public long CodGeo { get; set; }
        public int NumeroPosteNaOS { get; set; }
        public long IdOrdemServico { get; set; }
        public string IdOrdemServicoTexto { get; set; }

        public AlturaPoste? Altura { get; set; }
        public TipoPoste TipoPoste { get; set; }
        public Esforco? Esforco { get; set; }
        public string Descricao { get; set; }
        public string NumeroPrefeitura { get; set; }

        public string EncontradoPoste { get; set; }
        public string PararioPoste { get; set; }
        public string AterramentoPoste { get; set; }
        public string EstruturaPrimariaPoste { get; set; }
        public string EstruturaSecundaria_poste { get; set; }
        public int QuantidadeEstai { get; set; }
        public string AnoPoste { get; set; }
        public string SituacaoPoste { get; set; }
        public string EquipamentoPoste { get; set; }
        public string MuflaPoste { get; set; }
        public string RedePrimarioPoste { get; set; }
        public string DefeitoPoste { get; set; }
        public string BarramentoPoste { get; set; }
        public bool Finalizado { get; set; }

    }
}