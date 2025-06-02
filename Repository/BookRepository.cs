using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Data;
using DTOs;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Models.Book;

namespace Repository
{
    public class BookRepository : GenericRepository<Book>
    {
        private ProgramDbContent _content;
        public BookRepository(ProgramDbContent context) : base(context)
        {

            _content = context;
        }

        public void Delete(Book obj)
        {
            base.Delete(obj);
        }

        public async Task<IEnumerable<Book>> GetAll()
        {
            return await _content.Books.Include(book => book.Author)
                             .Include(book => book.Categories)
                             .Include(book => book.Publication)
                             .Include(book => book.FormType).ToListAsync();
        }

        public async Task<Book> GetById(Guid id)
        {
            var allBooks = await _content.Books.Include(book => book.Author)
                           .Include(book => book.Categories)
                           .Include(book => book.Publication)
                           .Include(book => book.FormType).ToListAsync();

            var book = allBooks.FirstOrDefault(book => book.Id == id);
            return book;
        }

        public async Task Insert(Book obj)
        {
            await base.Insert(obj);
        }

        public void Update(Book obj)
        {
            base.Update(obj);
        }
        public async Task<IEnumerable<Book>> Filterbook(filterBookDTO filterBook)
        {
            try
            {
                var categoryFilters = string.IsNullOrEmpty(filterBook.Category)
                                                        ? null
                                                        : filterBook.Category.Split(", ", StringSplitOptions.RemoveEmptyEntries);

                var books = await _content.Books
                                            .Include(book => book.Author)
                                            .Include(book => book.Categories)
                                            .Include(book => book.Publication)
                                            .Include(book => book.FormType)
                                            .Where(book => string.IsNullOrEmpty(filterBook.Name) || book.Title.Contains(filterBook.Name))
                                            .Where(book => string.IsNullOrEmpty(filterBook.Description) || book.Description.Contains(filterBook.Description))
                                            .Where(book => string.IsNullOrEmpty(filterBook.Language) || book.Language.Contains(filterBook.Language))
                                            .Where(book => string.IsNullOrEmpty(filterBook.Pages) || book.NoOfPages >= Convert.ToInt32(filterBook.Pages))
                                            .Where(book => string.IsNullOrEmpty(filterBook.Price) || book.Price >= Convert.ToDecimal(filterBook.Price))
                                            .Where(book => string.IsNullOrEmpty(filterBook.Author) || book.Author.Name == filterBook.Author)
                                            .Where(book => string.IsNullOrEmpty(filterBook.Publisher) || book.Publication.Name == filterBook.Publisher)
                                            .Where(book => string.IsNullOrEmpty(filterBook.form) || book.FormType.Name == filterBook.form)
                                             .Where(book => categoryFilters == null || book.Categories.Any(cat => categoryFilters.Contains(cat.Name)))
                                             .Skip(Convert.ToInt32(filterBook.pageSize) * Convert.ToInt32(filterBook.pageNumber))
                                             .Take(Convert.ToInt32(filterBook.pageSize))

                                            .ToListAsync();

                return books;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<int> getBooksCount(filterBookDTO filterBook)
        {
            try
            {
                var categoryFilters = string.IsNullOrEmpty(filterBook.Category)
                                                        ? null
                                                        : filterBook.Category.Split(", ", StringSplitOptions.RemoveEmptyEntries);

                var booksCount =  _content.Books
                                            .Include(book => book.Author)
                                            .Include(book => book.Categories)
                                            .Include(book => book.Publication)
                                            .Include(book => book.FormType)
                                            .Where(book => string.IsNullOrEmpty(filterBook.Name) || book.Title.Contains(filterBook.Name))
                                            .Where(book => string.IsNullOrEmpty(filterBook.Description) || book.Description.Contains(filterBook.Description))
                                            .Where(book => string.IsNullOrEmpty(filterBook.Language) || book.Language.Contains(filterBook.Language))
                                            .Where(book => string.IsNullOrEmpty(filterBook.Pages) || book.NoOfPages >= Convert.ToInt32(filterBook.Pages))
                                            .Where(book => string.IsNullOrEmpty(filterBook.Price) || book.Price >= Convert.ToDecimal(filterBook.Price))
                                            .Where(book => string.IsNullOrEmpty(filterBook.Author) || book.Author.Name == filterBook.Author)
                                            .Where(book => string.IsNullOrEmpty(filterBook.Publisher) || book.Publication.Name == filterBook.Publisher)
                                            .Where(book => string.IsNullOrEmpty(filterBook.form) || book.FormType.Name == filterBook.form)
                                             .Where(book => categoryFilters == null || book.Categories.Any(cat => categoryFilters.Contains(cat.Name)))

                                            .Count();

                return booksCount;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
