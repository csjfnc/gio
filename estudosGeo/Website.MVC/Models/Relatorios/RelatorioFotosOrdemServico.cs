using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Website.MVC.Models.Relatorios
{
    public class RelatorioFotosOrdemServico
    {
        public long CodGeo { get; set; }
        public long IdPoste { get; set; }
        public long? IdPoste_1 { get; set; }
        public double X { get; set; }
        public double Y { get; set; }
        public string Cidade { get; set; }
        public string OrdemServico { get; set; }
        public string Colaborador { get; set; }
        public string Fotos { get; set; }
        public DateTime? DataCadastro { get; set; }
    }
}