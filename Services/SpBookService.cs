using Data;
using DTOs;
using Microsoft.EntityFrameworkCore;
using Models.Book;
using Models.Shared;
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
                var allauthors = await _UnitOfWork.AuthorGenericRepo.GetAll();
                foreach (var item in allauthors)
                {
                    Console.WriteLine("Author is " + item.Name);

                }
                foreach (var book in result.books)
                {
                    var allCategories = await _UnitOfWork.categoriesGenericRepo.GetAll();

                    var category = new List<Category>();
                    if (!string.IsNullOrEmpty(filterData.Category))
                    {

                        category = allCategories
                             .Where(cat => filterData.Category.Split(", ").Contains(cat.Name))
                             .ToList();
                    }

                    var authors = await _UnitOfWork.AuthorGenericRepo.GetAll();
                    var author = authors.Where(auth => auth.Name == filterData.Author).FirstOrDefault();
                    Console.WriteLine("Author is ", authors);
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

        public async Task<Response> createOrEdit(createOrEditBook createOrEditBook)
        {
            try
            {
                var allAuthors = await _UnitOfWork.AuthorGenericRepo.GetAll();
                var author = allAuthors.Where(auth => auth.Name == createOrEditBook.Author).FirstOrDefault();
                if (author == null)
                {
                    return new Response
                    {
                        StatusCode = 1,
                        Message = "Author can not be found",
                        Data = string.Empty
                    };
                }
                Console.WriteLine("Author name is " + author.Id);

                var allPublication = await _UnitOfWork.PublicationGenericRepo.GetAll();
                var publication = allPublication.Where(pub => pub.Name == createOrEditBook.Publisher).FirstOrDefault();
                if (publication == null)
                {
                    return new Response
                    {
                        StatusCode = 1,
                        Message = "Publisher can not be found",
                        Data = string.Empty
                    };
                }
                Console.WriteLine("Publisher is " + publication.Name);


                var allCategories = await _UnitOfWork.categoriesGenericRepo.GetAll();
                var Categories = allCategories.Where(cat => (createOrEditBook.Categories).Split(",").Contains(cat.Name));
                if (Categories == null || Categories.Count() <= 0)
                {
                    return new Response
                    {
                        StatusCode = 1,
                        Message = "Categories can not be found",
                        Data = string.Empty
                    };
                }
                List<Guid> categoryNamesList = new List<Guid>();

                if (Categories != null && Categories.Any())
                {
                    foreach (var item in Categories)
                    {
                        Console.WriteLine(item.Name);
                        categoryNamesList.Add(item.Id);
                    }
                }

                var allFormTYpe = await _UnitOfWork.formTypeRepo.GetAll();
                var formType = allFormTYpe.Where(form => form.Name == createOrEditBook.Form).FirstOrDefault();
                if (formType == null)
                {
                    return new Response
                    {
                        StatusCode = 1,
                        Message = "FormType can not be found",
                        Data = string.Empty
                    };
                }
                Console.WriteLine("FormType is " + formType.Name);


                var createEditReq = new createEditReqDTO
                {
                    Id = createOrEditBook?.Id == Guid.Empty ? Guid.Empty : createOrEditBook?.Id ?? Guid.Empty,
                    Title = createOrEditBook?.Title,
                    Description = createOrEditBook?.Description,
                    Pages = createOrEditBook?.Pages,
                    Price = createOrEditBook?.Price,
                    Language = createOrEditBook?.Language,
                    AuthorId = author.Id,
                    PublisherId = publication.Id,
                    CategoriesTD = String.Join(",", categoryNamesList.Where(c => c != Guid.Empty)),
                    formTypeID = formType.Id
                };
                var result = await _bookrepository.createOrEdit(createEditReq);

                return new Response
                {
                    StatusCode = 0,
                    Message = "editOrCreateBook req executed successfully ",
                    Data = result
                };

            }
            catch (Exception ex)
            {
                throw;
            }
        }
    }
}
