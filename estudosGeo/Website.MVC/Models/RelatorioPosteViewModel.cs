using System;
using Website.BLL.Enums;

namespace Website.MVC.Models
{
    public class RelatorioPosteViewModel
    {
        public long CodGeo { get; set; }
        public long IdPoste { get; set; }
        public string StatusComparativo { get; set; }
        public double X { get; set; }
        public double Y { get; set; }
        public string Cidade { get; set; }
        public string OrdemServico { get; set; }
        public string Colaborador { get; set; }
        public DateTime? DataCadastro { get; set; }
        public AlturaPoste? Altura { get; set; }
        public string TipoPoste { get; set; }
        //public TipoPoste TipoPoste { get; set; }
        public Esforco? Esforco { get; set; }
        public string Descricao { get; set; }
        public int NumeroPosteNaOS { get; set; }
        public string node { get; set; }

        ///Campos do BD Geosurvey
        public string logradouro { get; set; }
        public string equipamento1 { get; set; }
        public string equipamento2 { get; set; }
        public string equipamento3 { get; set; }
        public string aterramento { get; set; }
        public string nomedobloco { get; set; }
        public int id_temp { get; set; }
        public int ativo { get; set; }
        public string primario { get; set; }
        public string proprietario { get; set; }
        public int qtdPoste { get; set; }
        public int idPotencia { get; set; }
        public string caracteristica { get; set; }
        public string aterropararraio { get; set; }
        public string encontrado { get; set; }
        public string material { get; set; }
        public string tipoBase { get; set; }
        public string paraRaio { get; set; }
        public string estai { get; set; }
        public int qtdRL { get; set; }
        public int qtdRS { get; set; }
        public int qtdEstai { get; set; }
        public string avaria { get; set; }
        public string ocupantes { get; set; }
        public int qtdOcupantes { get; set; }
        public int qtdDrop { get; set; }
        public string estai2 { get; set; }
        public int qtdEstai2 { get; set; }
        public int lampSemaforo { get; set; }
        public string tipoZona { get; set; }
        
    }
}