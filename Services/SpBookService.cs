using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Data;
using DTOs;
using Models.Book;
using Repository;

namespace Services
{
    public class SpBookService
    {
        private readonly IUnitOfWork _UnitOfWork;
        private readonly SpBookRepository _bookrepository;

        public SpBookService(IUnitOfWork unitOfWork, ProgramDbContent context)
        {
            _UnitOfWork = unitOfWork;
            _bookrepository = new SpBookRepository(context);

        }

        public async Task<(IEnumerable<getBookDTO> books, int Totalcount)> filterData(filterBookDTO filterData)
        {
            try
            {
                var result = await _bookrepository.Filterbook(filterData);


                List<getBookDTO> getbookResult = new List<getBookDTO>();

                foreach (var book in result.books)
                {
                    var allCategories = await _UnitOfWork.categoriesGenericRepo.GetAll();

                    var category = new List<Category>();
                    if (!string.IsNullOrEmpty(filterData.Category))
                    {

                       category =allCategories
                            .Where(cat => filterData.Category.Split(", ").Contains(cat.Name))
                            .ToList();
                    }

                    var authors = await _UnitOfWork.AuthorGenericRepo.GetAll();
                    var author = authors.Where(auth => auth.Name == filterData.Author).FirstOrDefault();

                    var AllPublications = await _UnitOfWork.PublicationGenericRepo.GetAll();
                    var publication = AllPublications.Where(pub => pub.Name == filterData.Publisher).FirstOrDefault();


                    var getbookdto = new getBookDTO(book.Id,
                                                    book.Title,
                                                    book.Description,
                                                    book.NoOfPages,
                                                    book.Price,
                                                    book.Language,
                                                    category,
                                                    author,
                                                    publication
                    );

                    getbookResult.Add(getbookdto);
                }
                Console.WriteLine(result.totalCount);
                return (getbookResult, result.totalCount);



            }
            catch (Exception ex)
            {
                throw;
            }
        }
    }
}
