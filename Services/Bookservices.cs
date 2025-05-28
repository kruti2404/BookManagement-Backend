using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Models.Shared;
using DTOs;
using Repository;
using Data;
using Microsoft.VisualBasic;
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
            Console.WriteLine("Book getting ");
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
                                            book.Categories,
                                            book.Author,
                                            book.Publication
            );

            return getbookdto;

        }

        public async Task<Response> EditBook(Guid id, editBookDto newBook)
        {
            Console.WriteLine("Id is: " + id);
            var result = await _bookrepository.GetById(id);

            if (result != null)
            {
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
                    //Data = result
                };
            }
            else
            {
                return new Response
                {
                    StatusCode = 1,
                    Message = "Book not found. ",
                };
            }
        }


    }
}
