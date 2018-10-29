using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Website.MVC.Models.Relatorios
{
    public class RelatorioControleDiario
    {
        public DateTime Cadastro { get; set; }
        public int TotalPoste {get;set;}
        public int TotalArvore { get; set; }
        public int TotalInvasao { get; set; }
        public string Colaborador { get; set; }
    }
}