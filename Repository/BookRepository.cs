using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Data;
using Microsoft.EntityFrameworkCore;
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

        public  async Task<IEnumerable<Book>> GetAll()
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
    }
}
