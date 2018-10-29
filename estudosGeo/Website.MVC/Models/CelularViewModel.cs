using System.ComponentModel.DataAnnotations;

namespace Website.MVC.Models
{
    public class CelularViewModel
    {
        public long Id { get; set; }

        [StringLength(15, MinimumLength = 15, ErrorMessage = "O Imei deve conter 15 digitos")]        
        public string Imei { get; set; }

        [StringLength(50, MinimumLength = 3, ErrorMessage = "A Nome deve conter entre 3 e 50 caracteres")]
        public string Name { get; set; }

        [StringLength(11, MinimumLength = 5, ErrorMessage = "A Número de telefone deve conter entre 5 a 11 digitos")]
        public string PhoneNumber { get; set; }

        public bool IsBlock { get; set; }
    }
}