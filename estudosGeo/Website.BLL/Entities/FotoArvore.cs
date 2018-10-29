using System;

namespace Website.BLL.Entities
{
    public class FotoArvore
    {
        public long IdFotoArvore { get; set; }
        public string NumeroFoto { get; set; }
        public string Path { get; set; }
        public long CodigoGeoBD { get; set; }
        public DateTime? DataExclusao { get; set; }
        public DateTime DataFoto { get; set; }

        public long IdArvore { get; set; }
        public virtual Arvore Arvore { get; set; }
    }
}
