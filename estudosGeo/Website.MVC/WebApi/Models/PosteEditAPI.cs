using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Website.BLL.Enums;
using Website.MVC.WebApi.OfflineMode;

namespace Website.MVC.WebApi.Models
{
    public class PosteEditAPI
    {
        public long IdPoste { get; set; }
        public bool Finalizado { get; set; }

        [Range(-90d, 90d)]
        public double Lat { get; set; }

        [Range(-180d, 180d)]
        public double Lon { get; set; }

        [Range(-90d, 90d)]
        public double LatAtualizacao { get; set; }

        [Range(-180d, 180d)]
        public double LonAtualizacao { get; set; }

        [Range(1, long.MaxValue, ErrorMessage = "O campo IdOrdemDeServico inválido ou nulo.")]
        public long IdOrdemDeServico { get; set; }
                        
        //public string Fotos { get; set; }

        public AlturaPoste? Altura { get; set; }

        public TipoPoste TipoPoste { get; set; }

        public Esforco? Esforco { get; set; }

        public string Descricao { get; set; }

        public DateTime DataDiretorio { get; set; }

        public long DataAtualizacao { get; set; }

        public List<FotoAPI> Fotos { get; set; }

        public bool TryValidate(out ICollection<ValidationResult> results)
        {
            results = new List<ValidationResult>();
            return Validator.TryValidateObject(this, new ValidationContext(this, serviceProvider: null, items: null), results, validateAllProperties: true);
        }
    }
}