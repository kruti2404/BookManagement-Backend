using Models.Book;

namespace Repository
{
    public interface IUnitOfWork
    {
        IGenericRepository<Book> BookGenericRepo { get; }
        IGenericRepository<Author> AuthorGenericRepo { get; }
        IGenericRepository<Category> categoriesGenericRepo { get; }
        IGenericRepository<Publication> PublicationGenericRepo { get; }
        IGenericRepository<FormType> formTypeRepo { get; }
        Task CreateTransaction();
        Task Commit();
        void RollBack();
    }  
}
