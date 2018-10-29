using System.Web.Mvc;

namespace Website.MVC.Models.Maps
{
    public class CidadeViewModel
    {
        public string Nome { get; set; }
        public long Id { get; set; }
        public SelectList Cidades { get; set; }
    }
}