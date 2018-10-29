using System;
using System.Collections.Generic;
using Website.BLL.Utils.Geocoding;

namespace Website.BLL.Entities
{
    public class Cidade
    {
        public Cidade()
        {
            OrdensDeServico = new List<OrdemDeServico>();
            LimitesCiade = new List<LimiteCidade>();
        }

        public long IdCidade { get; set; }
        public string Nome { get; set; }
        public int NorteOuSul { get; set; }
        public int Zona { get; set; }
        public int SetimoDigito { get; set; }
        public Datum Datum { get; set; }
        public string CidadeDiretorio { get; set; }
        public DateTime? DataExclusao { get; set; }
            
        public virtual ICollection<OrdemDeServico> OrdensDeServico { get; set; }
        public virtual ICollection<LimiteCidade> LimitesCiade { get; set; }
    }
}