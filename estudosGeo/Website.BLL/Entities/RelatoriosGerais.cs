using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks; 

namespace Website.BLL.Entities
{
   public class RelatoriosGerais
    {
       public List<StrandRelatorio> StrandRelatorios { get; set; }
       public List<AnotacaoRelatorio> AnotacaoRelatorios { get; set; }
       public List<EdificioRelatorio> EdificioRelatorios { get; set; }
       public List<TerrenoRelatorio> TerrenoRelatorios { get; set; }
       public List<ReisidenciaRelatorio> ReisidenciaRelatorios { get; set; }
       public List<ComercioRelatorio> ComercioRelatorios { get; set; }
       public List<TiepRelatorio> TiepRelatorios { get; set; }
       public List<object> PostesRelatorios { get; set; }

        public RelatoriosGerais()
        {
            this.StrandRelatorios = new List<StrandRelatorio>();
            this.AnotacaoRelatorios = new List<AnotacaoRelatorio>();
            this.EdificioRelatorios = new List<EdificioRelatorio>();
            this.TerrenoRelatorios = new List<TerrenoRelatorio>();
            this.ReisidenciaRelatorios = new List<ReisidenciaRelatorio>();
            this.ComercioRelatorios = new List<ComercioRelatorio>();
            this.TiepRelatorios = new List<TiepRelatorio>();
            this.PostesRelatorios = new List<object>();
        }
    }
}
