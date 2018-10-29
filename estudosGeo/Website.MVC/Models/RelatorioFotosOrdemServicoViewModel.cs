using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Website.MVC.Models
{
    public class RelatorioFotosOrdemServicoViewModel
    {
        public long CodGeo { get; set; }
        public long IdPoste { get; set; }
        public string StatusComparativo { get; set; }
        public double X { get; set; }
        public double Y { get; set; }
        public string Cidade { get; set; }
        public string OrdemServico { get; set; }
        public string Colaborador { get; set; }
        public string Fotos { get; set; }
        public DateTime? DataCadastro { get; set; }
        public int NumeroPosteNaOS { get; set; }
    }
}