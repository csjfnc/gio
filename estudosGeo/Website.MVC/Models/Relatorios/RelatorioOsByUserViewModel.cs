using System;

namespace Website.MVC.Models.Relatorios
{
    public class RelatorioOsByUserViewModel
    {
        public string OrdemServico { get; set; }
        public DateTime? DataEncerramento { get; set; }
        public DateTime? DataCriacao { get; set; }
        public int TotalPostes { get; set; }
        public int TotalArvores { get; set; }
        public int TotalInvasao { get; set; }
        public string PorcentagemFinalizados { get; set; }
        public string Colaborador { get; set; }
        public long IdOS { get; set; }
    }
}