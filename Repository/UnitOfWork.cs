using Data;
using Microsoft.EntityFrameworkCore.Storage;
using Models.Book;

namespace Repository
{
    public class UnitOfWork : IUnitOfWork
    {
        public IDbContextTransaction _TransactionObj = null;
        private readonly ProgramDbContent _context;
        public IGenericRepository<Book> BookGenericRepo { get; set; }
        public IGenericRepository<Author> AuthorGenericRepo { get; set; }
        public IGenericRepository<Category> categoriesGenericRepo { get; set;}
        public IGenericRepository<Publication> PublicationGenericRepo { get; set;}
        public IGenericRepository<FormType> formTypeRepo { get; set; }
        bool isDisposed;

        public UnitOfWork(ProgramDbContent context)
        {
            _context = context;
            BookGenericRepo = new GenericRepository<Book>(context);
            formTypeRepo = new GenericRepository<FormType>(context);
            AuthorGenericRepo = new GenericRepository<Author>(context);
            categoriesGenericRepo = new GenericRepository<Category>(context);
            PublicationGenericRepo = new GenericRepository<Publication>(context);
        }

        public async Task CreateTransaction()
        {
            _TransactionObj = await _context.Database.BeginTransactionAsync();
        }
        public async Task Commit()
        {
            await _context.SaveChangesAsync();
        }

        public void RollBack()
        {
            _context.Database.RollbackTransaction();
        }

        //public void Dispose()
        //{
        //    _context.Dispose();
        //}

        //public void Dispose(bool disposing)
        //{
        //    if (!isDisposed)
        //    {
        //        if (disposing)
        //        {
        //            _context.Dispose();
        //            GC.SuppressFinalize(this);
        //        }
        //        isDisposed = true;
        //    }

        //}
    }
}
