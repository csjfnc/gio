using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Website.MVC.Models
{
    public class UsuarioViewModel
    {
        public string IdUser { get; set; }
        public string NomeUser { get; set; }
        public string LoginUser { get; set; }

        [StringLength(20, MinimumLength = 6, ErrorMessage = "A Senha deve conter entre 6 e 20 caracteres")]
        public string Senha { get; set; }

        [StringLength(20, MinimumLength = 6, ErrorMessage = "A Senha deve conter entre 6 e 20 caracteres")]
        public string NovaSenha { get; set; }

        [StringLength(20, MinimumLength = 6, ErrorMessage = "A Senha deve conter entre 6 e 20 caracteres")]
        public string ConfirmarSenha { get; set; }

        public List<CheckViewModel> CheckBoxModulos { get; set; }
        public List<CheckViewModel> CheckBoxPermissoes { get; set; }
    }
}