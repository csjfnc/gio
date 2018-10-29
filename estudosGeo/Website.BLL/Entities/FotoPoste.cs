using System;

namespace Website.BLL.Entities
{
    public class FotoPoste
    {
        public long IdFotoPoste { get; set; }
        public string NumeroFoto { get; set; }
        public string Path { get; set; }
        public long CodigoGeoBD { get; set; }
        public DateTime? DataExclusao { get; set; }
        public DateTime DataFoto { get; set; }

        public long IdPoste { get; set; }
        public virtual Poste Poste { get; set; }
    }
}