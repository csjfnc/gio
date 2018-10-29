using System;

namespace Website.BLL.Entities
{
    public class IP
    {
        public long IdIp { get; set; }
        public string TipoBraco { get; set; }
        public string TipoLuminaria { get; set; }
        public int? QtdLuminaria { get; set; }
        public string TipoLampada { get; set; }
        public double? Potencia { get; set; }
        public long CodigoGeoBD { get; set; }
        public string Fase { get; set; }
        public string Acionamento { get; set; }
        public string LampadaAcesa { get; set; }
        public int QtdLampada { get; set; }
        public DateTime? DataExclusao { get; set; }
        public string StatusComparativo { get; set; }
        public string NumeroPrefeitura { get; set; }

        public long IdPoste { get; set; }
        public virtual Poste Poste { get; set; }
        
        public long? IdLogradouro { get; set; }
        public virtual Logradouro Logradouro { get; set; }
    }
}
