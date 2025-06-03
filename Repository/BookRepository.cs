using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Text;
using System.Threading.Tasks;
using Data;
using DTOs;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Models.Book;
using static System.Reflection.Metadata.BlobBuilder;

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
        public async Task<IEnumerable<Book>> Filterbook(filterBookDTO filterBook, bool descending)
        {
            try
            {
                var categoryFilters = string.IsNullOrEmpty(filterBook.Category)
                                        ? null
                                        : filterBook.Category.Split(", ", StringSplitOptions.RemoveEmptyEntries);


                IQueryable<Book> query = _content.Books;

                query = query.Include(book => book.Author)
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
                             .Where(book => string.IsNullOrEmpty(filterBook.form) || book.FormType.Name == filterBook.form);

                if (filterBook.Category != null && filterBook.Category.Any())
                {
                    query = query.Where(book => book.Categories
                        .Any(cat => filterBook.Category.Contains(
                            EF.Functions.Collate(cat.Name, "SQL_Latin1_General_CP1_CI_AS")
                        )));
                }
                var books = await query.ToListAsync();


                if (!string.IsNullOrEmpty(filterBook.sortColumn))
                {
                    books = filterBook.sortColumn switch
                    {
                        "Title" => descending
                            ? books.OrderByDescending(b => b.Title).ToList()
                            : books.OrderBy(b => b.Title).ToList(), 
                        "Language" => descending
                            ? books.OrderByDescending(b => b.Language).ToList()
                            : books.OrderBy(b => b.Language).ToList(),
                        "NoOfPages" => descending
                            ? books.OrderByDescending(b => b.NoOfPages).ToList()
                            : books.OrderBy(b => b.NoOfPages).ToList(),
                        "Price" => descending
                             ? books.OrderByDescending(b => b.Price).ToList()
                             : books.OrderBy(b => b.Price).ToList(),
                        "Author" => descending
                            ? books.OrderByDescending(b => b.Author?.Name).ToList()
                            : books.OrderBy(b => b.Author?.Name).ToList(),
                        "Publication" => descending
                            ? books.OrderByDescending(b => b.Publication?.Name).ToList()
                            : books.OrderBy(b => b.Publication?.Name).ToList(),

                        _ => books
                    };
                }

                int pageSize = Convert.ToInt32(filterBook.pageSize);
                int pageNumber = Convert.ToInt32(filterBook.pageNumber);
                Console.WriteLine("Page size and page number " + pageSize + " " + pageNumber);
                books = books.Skip(pageSize * pageNumber).Take(pageSize).ToList();
                Console.WriteLine(books);
                foreach (var item in books)
                {
                    Console.WriteLine(item);
                }
                return books;
            }
            catch (Exception ex)
            {
                throw;
            }
        }


        public async Task<int> getBooksCount(filterBookDTO filterBook)
        {
            try
            {
                var categoryFilters = string.IsNullOrEmpty(filterBook.Category)
                                                        ? null
                                                        : filterBook.Category.Split(", ", StringSplitOptions.RemoveEmptyEntries);

                var booksCount = _content.Books
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
