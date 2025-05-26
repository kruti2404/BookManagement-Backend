using BookManagement_Backend.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;

namespace BookManagement_Backend.Repository
{
    public class UnitOfWork : IUnitOfWork
    {
        public IDbContextTransaction _TransactionObj = null;
        private readonly ProgramDbContent _context;
        bool isDisposed;
        public UnitOfWork(ProgramDbContent context)
        {
            _context = context;
        }

        public async Task CreateTransaction()
        {
            _TransactionObj = await _context.Database.BeginTransactionAsync();
        }

        public void Dispose()
        {
            _context.Dispose();
        }

        public void Dispose(bool disposing)
        {
            if (!isDisposed)
            {
                if (disposing)
                {
                    _context.Dispose();
                    GC.SuppressFinalize(this);
                }
                isDisposed = true;
            }

        }
    }
}
