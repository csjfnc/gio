using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Website.MVC.WebApi.Models
{
    public class IPEditAPI
    {
        public long IdIp { get; set; }
        public long IdPoste { get; set; }

        [Required(AllowEmptyStrings = false)]
        public string TipoBraco { get; set; }

        [Required(AllowEmptyStrings = false)]
        public string TipoLuminaria { get; set; }

        [Range(0, int.MaxValue)]
        public int QtdLuminaria { get; set; }

        [Required(AllowEmptyStrings = false)]
        public string TipoLampada { get; set; }

        [Range(0d, double.MaxValue)]
        public double Potencia { get; set; }

        [Required(AllowEmptyStrings = false)]
        public string Fase { get; set; }

        [Required(AllowEmptyStrings = false)]
        public string Acionamento { get; set; }

        [Required(AllowEmptyStrings = false)]
        public string LampadaAcesa { get; set; }

        [Range(-90d, 90d)]
        public double LatAtualizacao { get; set; }

        [Range(-180d, 180d)]
        public double LonAtualizacao { get; set; }

        [Range(0, int.MaxValue)]
        public int QtdLampada { get; set; }

        public bool TryValidate(out ICollection<ValidationResult> results)
        {
            results = new List<ValidationResult>();
            return Validator.TryValidateObject(this, new ValidationContext(this, serviceProvider: null, items: null), results, validateAllProperties: true);
        }
    }
}