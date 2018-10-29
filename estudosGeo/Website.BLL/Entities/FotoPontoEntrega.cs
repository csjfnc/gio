using System;

namespace Website.BLL.Entities
{
    public class FotoPontoEntrega
    {
        public long IdFotoPontoEntrega { get; set; }
        public string NumeroFoto { get; set; }
        public string Path { get; set; }
        public long CodigoGeoBD { get; set; }
        public DateTime? DataExclusao { get; set; }
        public DateTime DataFoto { get; set; }

        public long IdPontoEntrega { get; set; }
        public virtual PontoEntrega PontoEntrega { get; set; }
    }
}