using System;
using Website.DAL.Context;
using Website.DAL.Interfaces;
using Website.DAL.Repository;

namespace Website.DAL.UnitOfWork
{
    public class UnitOfWork : IDisposable
    {
        public UnitOfWork() {

            appContext = new AppContext();

            // Log para Debug
            // appContext.Database.Log = s => System.Diagnostics.Debug.WriteLine(s);
        }

        private AppContext appContext;
        private ICidadeRepository cidadeRepository;
        private IPosteRepository posteRepository;
        private IPoligonoOSRepository poligonoOSRepository;
        private IOrdemDeServicoRepository ordemDeServicoRepository;
        private IUsuarioRepository usuarioRepository;
        private IIPRepository ipRepository;
        private IFotoPosteRepository iFotoPosteRepository;
        private ILogradouroRepository iLogradouroRepository;
        private ITransformadorRepository iTransformadorRepository;
        private ILimiteCidadeRepository iLimiteCidadeRepository;
        private IVaoPrimarioRepository iVaoPrimarioRepository;
        private IPontoEntregaRepository peRepository;
        private IMedidorRepository meRepository;
        private IFotoPontoEntregaRepository fpeRepository;
        private IArvoreRepository arvRepository;
        private IFotoArvoreRepository ftarvRepository;
        private IVaosDemandaPosteRepository iVaosDemandaPosteRepository;
        private IAnotacaoRepository iAnotacaoRepository;
        private IDemandaStrandRepository iDemandaStrandRepository;
        private IQuadraRepository iQuadrasRepository;
        private ILatLonQuadraRepository iLatLonQuadraRepository;
        private IPublicacaoOrdemColaboradorRepository iPublicacaoOrdemColaboradorRepository;
        private ILocalUsuarioAtualRepository iLocalUsuarioAtualRepository;

        public ILocalUsuarioAtualRepository LocalUsuarioAtualRepository
        {
            get { return iLocalUsuarioAtualRepository ?? (iLocalUsuarioAtualRepository = new LocalUsuarioAtualRepository(appContext));}
        }

        public IPublicacaoOrdemColaboradorRepository PublicacaoOrdemColaboradorRepository
        {
            get { return iPublicacaoOrdemColaboradorRepository ?? (iPublicacaoOrdemColaboradorRepository = new PublicacaoOrdemColaboradorRepository(appContext));}
        }

        public ILatLonQuadraRepository LatLonQuadraRepository{
            get {return iLatLonQuadraRepository ?? (iLatLonQuadraRepository = new LatLonQuadraRepository(appContext));}
        }
        
        public  IQuadraRepository QuadrasRepository{
            get {return iQuadrasRepository ?? (iQuadrasRepository = new QuadraRepository(appContext));}
        }

        public IDemandaStrandRepository DemandaStrandRepository
        {
            get{return iDemandaStrandRepository ?? (iDemandaStrandRepository = new DemandaStrandRepository(appContext));}
        }

        public IAnotacaoRepository AnotacaoRepository
        {
            get{ return iAnotacaoRepository ?? (iAnotacaoRepository = new AnotacaoRepository(appContext));}
        }
        public IVaosDemandaPosteRepository VaosDemandaPosteRepository
        {
            get { return iVaosDemandaPosteRepository ?? (iVaosDemandaPosteRepository = new VaosDemandaPosteRespository(appContext));}
        }
        public IFotoPontoEntregaRepository FotoPontoEntregaRepository
        {
            get { return fpeRepository ?? (fpeRepository = new FotoPontoEntregaRepository(appContext)); }
        }

        public IMedidorRepository MedidoresRepository
        {
            get { return meRepository ?? (meRepository = new MedidorRepository(appContext)); }
        }

        public IPontoEntregaRepository PontoEntregaRepository
        {
            get { return peRepository ?? (peRepository = new PontoEntregaRepository(appContext)); }
        }

        public IVaoPrimarioRepository VaoPrimarioRepository
        {
            get { return iVaoPrimarioRepository ?? (iVaoPrimarioRepository = new VaoPrimarioRepository(appContext)); }
        }

       

        public ICidadeRepository CidadeRepository
        {
            get { return cidadeRepository ?? (cidadeRepository = new CidadeRepository(appContext)); }
        }
        public IPosteRepository PosteRepository
        {
            get { return posteRepository ?? (posteRepository = new PosteRepository(appContext)); }
        }
        public IPoligonoOSRepository PoligonoOSRepository
        {
            get { return poligonoOSRepository ?? (poligonoOSRepository = new PoligonoOSRepository(appContext)); }
        }
        public IOrdemDeServicoRepository OrdemDeServicoRepository
        {
            get { return ordemDeServicoRepository ?? (ordemDeServicoRepository = new OrdemDeServicoRepository(appContext)); }
        }
        public IUsuarioRepository UsuarioRepository
        {
            get { return usuarioRepository ?? (usuarioRepository = new UsuarioRepository(appContext)); }
        }
        public IIPRepository IPRepository
        {
            get { return ipRepository ?? (ipRepository = new IPRepository(appContext)); }
        }
        public IFotoPosteRepository FotoPosteRepository
        {
            get { return iFotoPosteRepository ?? (iFotoPosteRepository = new FotoPosteRepository(appContext)); }
        }
        public ILogradouroRepository LogradouroRepository
        {
            get { return iLogradouroRepository ?? (iLogradouroRepository = new LogradouroRepository(appContext)); }
        }
        public ITransformadorRepository TransformadorRepository
        {
            get { return iTransformadorRepository ?? (iTransformadorRepository = new TransformadorRepository(appContext)); }
        }
        public ILimiteCidadeRepository LimiteCidadeRepository 
        {
            get { return iLimiteCidadeRepository ?? (iLimiteCidadeRepository = new LimiteCidadeRepository(appContext)); }
        }

        public IArvoreRepository ArvoreRepository
        {
            get { return arvRepository ?? (arvRepository = new ArvoreRepository(appContext)); }
        }

        public IFotoArvoreRepository FotoArvoreRepository
        {
            get { return ftarvRepository ?? (ftarvRepository = new FotoArvoreRepository(appContext)); }
        }

        public void Save()
        {
            appContext.SaveChanges();
        }

        public void SaveAsync()
        {
            appContext.SaveChangesAsync();
        }

        private bool disposed = false;
        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    appContext.Dispose();
                }
            }
            this.disposed = true;
        }
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}