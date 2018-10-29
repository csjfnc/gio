using System;
namespace Website.MVC.WebApi.Models
{
    public class MedidorAPI
    {   
        public long IdMedidor { get; set; }
        public string NumeroMedidor { get; set; }
        public string ComplementoResidencial { get; set; }
        public DateTime? DataExclusao { get; set; }
    }
}