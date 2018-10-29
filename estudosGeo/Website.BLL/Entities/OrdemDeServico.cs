using System;
using System.Collections.Generic;
using Website.BLL.Enums;

namespace Website.BLL.Entities
{
    public class OrdemDeServico
    {
        public OrdemDeServico()
        {
            PoligonosOS = new List<PoligonoOS>();
            Postes = new List<Poste>();
        }

        public long IdOrdemDeServico { get; set; }
        public string NumeroOS { get; set; }
        public SituacaoOrdemServico Situacao { get; set; }
        public DateTime? DataInicio { get; set; }
        public DateTime? DataFinal { get; set; }
        public DateTime? DataPublicacao { get; set; }     
        public string Observacao { get; set; }
        public string IdUsuario { get; set; }
        public virtual Usuario Usuario { get; set; }         
        public long IdCidade { get; set; }
        public virtual Cidade Cidade { get; set; }
        public virtual ICollection<PoligonoOS> PoligonosOS { get; set; }
        public virtual ICollection<Poste> Postes { get; set; }
        public virtual ICollection<VaoPrimario> Vaos { get; set; }
        public virtual ICollection<PontoEntrega> PontoEntregas { get; set; }

    }
}
