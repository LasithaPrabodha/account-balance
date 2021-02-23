using Contracts;
using Entities;
using System;
using System.Threading.Tasks;

namespace Repository
{
    public class UnitOfWork : IUnitOfWork, IDisposable
    {
        private RepositoryContext _context;
        private ITransactionRepository _transaction;
        private IAccountRepository _account;
        public UnitOfWork(RepositoryContext repositoryContext)
        {
            _context = repositoryContext;
        }

        public IAccountRepository Account
        {
            get
            {
                if (_account == null)
                {
                    _account = new AccountRepository(_context);
                }

                return _account;
            }
        }

        public ITransactionRepository Transaction
        {
            get
            {
                if (_transaction == null)
                {
                    _transaction = new TransactionRepository(_context);
                }

                return _transaction;
            }
        }

        public async Task Save()
        {
            await _context.SaveChangesAsync();
        }

        private bool disposed = false;

        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    _context.Dispose();
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