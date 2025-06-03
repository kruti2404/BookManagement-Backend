using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Data;
using DTOs;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Models.Book;

namespace Repository
{
    public class SpBookRepository : GenericRepository<Book>
    {
        private ProgramDbContent _Context;
        public SpBookRepository(ProgramDbContent context) : base(context)
        {
            _Context = context;
        }

        public void Delete(Book obj)
        {
            //base.Delete(obj);
        }

        public async Task Insert(Book obj)
        {
            await base.Insert(obj);
        }

        public void Update(Book obj)
        {
            base.Update(obj);
        }
        public async Task<(List<FilteredBookDTO> books, int totalCount)> Filterbook(filterBookDTO filterBook)
        {
            try
            {
                var parameters = new List<SqlParameter>
                                    {
                                        new SqlParameter("@Name", filterBook.Name ?? ""),
                                        new SqlParameter("@Description", filterBook.Description ?? ""),
                                        new SqlParameter("@Pages", string.IsNullOrEmpty(filterBook.Pages) ? 0 : Convert.ToInt32(filterBook.Pages)),
                                        new SqlParameter("@Price", string.IsNullOrEmpty(filterBook.Price) ? 0 : Convert.ToInt32(filterBook.Price)),
                                        new SqlParameter("@Language", filterBook.Language ?? ""),
                                        new SqlParameter("@Author", filterBook.Author ?? ""),
                                        new SqlParameter("@Category", filterBook.Category ?? ""),
                                        new SqlParameter("@Publisher", filterBook.Publisher ?? ""),
                                        new SqlParameter("@form", filterBook.form ?? ""),
                                        new SqlParameter("@pageNumber", Convert.ToInt32(filterBook.pageNumber)),
                                        new SqlParameter("@pageSize", Convert.ToInt32(filterBook.pageSize)),
                                        new SqlParameter("@sortColumn", filterBook.sortColumn ?? "Title"),
                                        new SqlParameter("@sortDirection", filterBook.sortDirection ?? "ASC"),

                                        // OUTPUT parameter
                                        new SqlParameter
                                        {
                                            ParameterName = "@TotalCount",
                                            SqlDbType = SqlDbType.Int,
                                            Direction = ParameterDirection.Output
                                        }
                };

                // Call stored procedure
                var books = await _Context.FilteredBookDTOs
                    .FromSqlRaw("EXEC FilterBook @Name, @Description, @Pages, @Price, @Language, @Author, @Category, @Publisher, @form, @pageNumber, @pageSize, @sortColumn, @sortDirection, @TotalCount OUTPUT", parameters.ToArray())
                    .ToListAsync();

                int totalCount = (int)parameters.Last().Value;
                Console.WriteLine("The count is " + totalCount);
                Console.WriteLine("The Result is " + books);
                
                return (books, totalCount);
            }
            catch (Exception)
            {
                throw;
            }
        }

    }
}
