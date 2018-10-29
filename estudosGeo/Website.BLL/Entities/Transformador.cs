using System;

namespace Website.BLL.Entities
{
    public class Transformador
    {
        public long IdTransformador { get; set; }
        public long CodigoGeoBD { get; set; }
        public string Status { get; set; }
        public string Proprietario { get; set; }
        public string Fase { get; set; }
        public string NumeroCampo { get; set; }
        public string PotenciaTotal { get; set; }
        public string TipoLigacao { get; set; }
        public int TensaoNominal { get; set; }
        public string TipoInstalacao { get; set; }
        public string CortaCircuito { get; set; }
        public string Descricao { get; set; }
        public int NumeroEquipamento { get; set; }

        public long IdPoste { get; set; }
        public virtual Poste Poste { get; set; }

        public DateTime? DataExclusao { get; set; }
    }
}