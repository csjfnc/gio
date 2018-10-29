using Website.BLL.Entities;
using Website.MVC.WebApi.Models;

namespace Website.MVC.WebApi.Bussiness
{
    public static class CalculaStatusOs
    {
        public static StatusOs Calcular(OrdemDeServico OS)
        {
            return OS.DataFinal != null ? StatusOs.VERDE : (OS.IdUsuario != null && OS.IdUsuario.Equals("0")) ? StatusOs.VERMELHO : StatusOs.AZUL;
        }
    }
}