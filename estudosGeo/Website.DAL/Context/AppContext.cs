using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using Website.DAL.EntityConfig;

namespace Website.DAL.Context
{
    public class AppContext : DbContext
    {
        public AppContext() : base("DefaultConnection")
        {
            /// Desabilitando o LazyLoading
            Configuration.LazyLoadingEnabled = false;
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Conventions.Remove<OneToManyCascadeDeleteConvention>();
            modelBuilder.Conventions.Remove<ManyToManyCascadeDeleteConvention>();

            modelBuilder.Configurations.Add(new CidadeConfig());
            modelBuilder.Configurations.Add(new PoligonosOSConfig());
            modelBuilder.Configurations.Add(new PosteConfig());
            modelBuilder.Configurations.Add(new OrdemDeServicoConfig());
            modelBuilder.Configurations.Add(new IPConfig());
            modelBuilder.Configurations.Add(new UsuarioConfig());
            modelBuilder.Configurations.Add(new FotoPosteConfig());
            modelBuilder.Configurations.Add(new LogradouroConfig());
            modelBuilder.Configurations.Add(new TransformadorConfig());
            modelBuilder.Configurations.Add(new LimiteCidadeConfig());
            modelBuilder.Configurations.Add(new VaoPrimarioConfig());
            modelBuilder.Configurations.Add(new PontoEntregaConfig());
            modelBuilder.Configurations.Add(new MedidorConfig());
            modelBuilder.Configurations.Add(new FotoPontoEntregaConfig());
            modelBuilder.Configurations.Add(new ArvoreConfig());
            modelBuilder.Configurations.Add(new FotoArvoreConfig());
            modelBuilder.Configurations.Add(new VaosDemandaPosteConfig());
            modelBuilder.Configurations.Add(new AnotacaoConfig());
            modelBuilder.Configurations.Add(new DemandaStrandConfig());
            modelBuilder.Configurations.Add(new QuadraConfig());
            modelBuilder.Configurations.Add(new LatLonQuadraConfig());
            modelBuilder.Configurations.Add(new PublicacaoOrdemColaboradorConfig());
            modelBuilder.Configurations.Add(new LocalUsuarioAtualConfig());
            /// Add as configuraçoes das outras Entidades

            base.OnModelCreating(modelBuilder);
        }
    }
}