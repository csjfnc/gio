using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Website.MVC.WebApi.Models
{
    public class TransformadorEditAPI
    {
        public long IdTransformador { get; set; }
        public long IdPoste { get; set; }
                
        public long CodigoGeoBD { get; set; }

        [Required(AllowEmptyStrings = false)]
        public string Status { get; set; }

        [Required(AllowEmptyStrings = false)]
        public string Proprietario { get; set; }

        [Required(AllowEmptyStrings = false)]
        public string Fase { get; set; }

        [Required(AllowEmptyStrings = false)]
        public string NumeroCampo { get; set; }

        [Required(AllowEmptyStrings = false)]
        public string PotenciaTotal { get; set; }

        [Required(AllowEmptyStrings = false)]
        public string TipoLigacao { get; set; }

        [Range(0, int.MaxValue)]
        public int TensaoNominal { get; set; }

        [Required(AllowEmptyStrings = false)]
        public string TipoInstalacao { get; set; }

        [Required(AllowEmptyStrings = false)]
        public string CortaCircuito { get; set; }
                
        public string Descricao { get; set; }

        [Range(0, int.MaxValue)]
        public int NumeroEquipamento { get; set; }

        [Range(-90d, 90d)]
        public double LatAtualizacao { get; set; }

        [Range(-180d, 180d)]
        public double LonAtualizacao { get; set; }

        public bool TryValidate(out ICollection<ValidationResult> results)
        {
            results = new List<ValidationResult>();
            return Validator.TryValidateObject(this, new ValidationContext(this, serviceProvider: null, items: null), results, validateAllProperties: true);
        }
    }
}