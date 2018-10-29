namespace Website.BLL.Entities
{
    public class LimiteCidade
    {
        public long IdLimeteCidade { get; set; }
        public double X1 { get; set; }
        public double Y1 { get; set; }
        public double X2 { get; set; }
        public double Y2 { get; set; }

        public long? CodigoGeoBD { get; set; }

        public long IdCidade { get; set; }
        public virtual Cidade Cidade { get; set; }
    }
}
