using System;
using Website.DAL.Context;
using Website.DAL.Interfaces;
using Website.DAL.Repository;

namespace Website.DAL.UnitOfWork
{
    public class UnitOfWorkMobile : IDisposable
    {
        public UnitOfWorkMobile() { context = new MobileContext(); }

        private MobileContext context;

        private IMobileRepository repository;
        public IMobileRepository Repository
        {
            get { return repository ?? (repository = new MobileRepository(context)); }
        }

        private bool disposed = false;
        protected virtual void Dispose(bool disposing)
        {
            if (!disposed)
            {
                if (disposing)
                {
                    context.Dispose();
                }
            }
            disposed = true;
        }
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}