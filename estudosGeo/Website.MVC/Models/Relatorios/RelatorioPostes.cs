using System;
using Website.BLL.Enums;

namespace Website.MVC.Models.Relatorios
{
    public class RelatorioPostes
    {
        public long CodGeo { get; set; }
        public long IdPoste { get; set; }
        public long? IdPoste_1 { get; set; }
        public double X { get; set; }
        public double Y { get; set; }
        public string Cidade { get; set; }
        public string OrdemServico { get; set; }
        public string Colaborador { get; set; }
        public DateTime? DataCadastro { get; set; }
        public double? Altura { get; set; }
        public string TipoPoste { get; set; }
        public string Esforco { get; set; }
        public string Descricao { get; set; }
    }
}