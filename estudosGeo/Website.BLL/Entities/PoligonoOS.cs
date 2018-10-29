namespace Website.BLL.Entities
{
    public class PoligonoOS
    {
        public long IdPoligonoOS { get; set; }
        public double X1 { get; set; }
        public double Y1 { get; set; }
        public double X2 { get; set; }
        public double Y2 { get; set; }

        public int Ordem { get; set; }

        public long CodigoGeoBD { get; set; }

        public long IdOrdemDeServico { get; set; }
        public virtual OrdemDeServico OrdemDeServico { get; set; }
    }
}