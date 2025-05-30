using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Models.Shared;
using DTOs;
using Repository;
using Data;
using Microsoft.VisualBasic;
using Models.Book;
using Microsoft.EntityFrameworkCore.Diagnostics;
namespace Services
{
    public class Bookservices
    {
        private readonly IUnitOfWork _uowInstance;

        private BookRepository _bookrepository { get; set; }
        public Bookservices(IUnitOfWork uowInstance, ProgramDbContent context)
        {
            _uowInstance = uowInstance;
            _bookrepository = new BookRepository(context);
        }
        public async Task<IEnumerable<getBookDTO>> getBooks(string FormtypeName)
        {
            var allBooks = await _bookrepository.GetAll();
            var FilteredResult = allBooks.Where(book => book.FormType.Name == FormtypeName);
            if (FilteredResult.Count() < 1)
            {
                return null;
            }
            List<getBookDTO> result = new List<getBookDTO>();
            foreach (var book in FilteredResult)
            {
                var getbookdto = new getBookDTO(book.Id,
                                                book.Title,
                                                book.Description,
                                                book.NoOfPages,
                                                book.Price,
                                            book.Language,
                                                book.Categories,
                                                book.Author,
                                                book.Publication
                );

                result.Add(getbookdto);
            }
            return result;
        }


        public async Task<getBookDTO> getBookById(Guid id)
        {
            Console.WriteLine("Book getting ", id);
            var book = await _bookrepository.GetById(id);
            if (book == null)
            {
                Console.WriteLine("Book not Found");
                return null;
            }
            Console.WriteLine(book);
            var getbookdto = new getBookDTO(book.Id,
                                            book.Title,
                                            book.Description,
                                            book.NoOfPages,
                                            book.Price,
                                            book.Language,
                                            book.Categories,
                                            book.Author,
                                            book.Publication
            );

            return getbookdto;

        }

        public async Task<Response> EditBook(Guid id, editBookDto newBook)
        {
            try
            {
                Console.WriteLine("Id is: " + id);
                var result = await _bookrepository.GetById(id);

                if (result == null)
                {
                    return new Response
                    {
                        StatusCode = 1,
                        Message = "Book not found. ",
                    };
                }
                Console.WriteLine("Editing the book...");

                result.Title = newBook.Title;
                result.Description = newBook.Description;
                result.NoOfPages = newBook.Pages;

                Console.WriteLine("Author is " + newBook.AuthorName);

                var allAuthor = await _uowInstance.AuthorGenericRepo.GetAll();


                var author = allAuthor.FirstOrDefault(auth => auth.Name == newBook.AuthorName);

                if (author == null)
                {
                    Console.WriteLine("Author not found.");
                    return new Response
                    {
                        StatusCode = 1,
                        Message = "Author not found.",
                    };
                }

                result.Author = author;

                var categoryNames = newBook.CategoriesName.Split(", ", StringSplitOptions.RemoveEmptyEntries);
                var Categories = (await _uowInstance.categoriesGenericRepo.GetAll()).Where(cat => categoryNames.Contains(cat.Name)).ToList();
                if (Categories.Count == 0)
                {
                    Console.WriteLine("Categories not Found");
                    return new Response
                    {
                        StatusCode = 1,
                        Message = "Categories not Found",
                    };
                }
                else if (categoryNames.Length != Categories.Count)
                {
                    Console.WriteLine("Any one of the Categories could not Found");
                    return new Response
                    {
                        StatusCode = 1,
                        Message = "Any one of the Categories could not Found",
                    };
                }
                result.Categories = Categories;

                var publication = (await _uowInstance.PublicationGenericRepo.GetAll()).FirstOrDefault(publication => publication.Name == newBook.PublicationName);
                if (publication == null)
                {
                    Console.WriteLine("Publication not Found");
                    return new Response
                    {
                        StatusCode = 1,
                        Message = "Publication not Found",
                    };
                }

                result.Publication = publication;
                await _uowInstance.Commit();

                return new Response
                {
                    StatusCode = 0,
                    Message = "Book edited Successfully",
                };

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public async Task<Response> deleteBook(Guid id)
        {
            Console.WriteLine("deltebook at the service with id ", id);
            var book = await _bookrepository.GetById(id);
            if (book == null)
            {
                return new Response
                {
                    StatusCode = 1,
                    Message = "Book not found"
                };
            }
            try
            {
                _bookrepository.Delete(book);
                await _uowInstance.Commit();
                return new Response
                {
                    StatusCode = 0,
                    Message = "Data delted Successfully"
                };
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        
        public async Task<Response> createBook( createBookDTO createBook)
        {
            try
            {
                Console.WriteLine("CreateBook of the service ");


                var allAuthor = await _uowInstance.AuthorGenericRepo.GetAll();
                var author = allAuthor.FirstOrDefault(auth => auth.Name == createBook.Author);

                if (author == null)
                {
                    Console.WriteLine("Author not found.");
                    return new Response
                    {
                        StatusCode = 1,
                        Message = "Author not found.",
                    };
                }

                var categoryNames = createBook.Categories.Split(", ", StringSplitOptions.RemoveEmptyEntries);
                var Categories = (await _uowInstance.categoriesGenericRepo.GetAll()).Where(cat => categoryNames.Contains(cat.Name)).ToList();

                if (Categories.Count == 0)
                {
                    Console.WriteLine("Categories not Found");
                    return new Response
                    {
                        StatusCode = 1,
                        Message = "Categories not Found",
                    };
                }
                else if (categoryNames.Length != Categories.Count)
                {
                    Console.WriteLine("Any one of the Categories could not Found");
                    return new Response
                    {
                        StatusCode = 1,
                        Message = "Any one of the Categories could not Found",
                    };
                }

                var publication = (await _uowInstance.PublicationGenericRepo.GetAll()).FirstOrDefault(publication => publication.Name == createBook.Publisher);
                if (publication == null)
                {
                    Console.WriteLine("Publication could not Found");
                    return new Response
                    {
                        StatusCode = 1,
                        Message = "Publication could not Found",
                    };
                }

                var allFormTypes = await _uowInstance.formTypeRepo.GetAll();
                var formType = allFormTypes.FirstOrDefault(formtype => formtype.Name == createBook.FormType);
                if (formType == null)
                {
                    Console.WriteLine("Specified wrong FormType ");
                    return new Response
                    {
                        StatusCode = 1,
                        Message = "Specified Wrong Form type.",
                    };
                }

                var newBook = new Book
                {
                    Id = new Guid(),
                    Title = createBook.Title,
                    Description = createBook.Description,
                    NoOfPages = Convert.ToInt32(createBook.Pages),
                    Price = Convert.ToInt32(createBook.Price),
                    Language = createBook.Language,
                    Categories = Categories,
                    PublicationId = publication.Id,
                    Publication = publication,
                    AuthorId = author.Id,
                    Author = author,
                    FormTypeId = formType.Id,
                    FormType = formType,
                };

                await _uowInstance.BookGenericRepo.Insert(newBook);
                await _uowInstance.Commit();

                return new Response
                {
                    StatusCode = 0,
                    Message = "Successfully created the new Book"

                };

            }
            catch (Exception ex)
            {
                //_uowInstance.RollBack();
                throw ex;
            }

        }

    }
}
