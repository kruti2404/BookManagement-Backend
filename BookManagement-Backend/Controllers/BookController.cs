using Microsoft.AspNetCore.Mvc;
using Services;
using DTOs;
using Models.Shared;
using System.Threading.Tasks;
using Azure.Core;
namespace BookManagement_Backend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BookController : Controller
    {
        private readonly Bookservices _bookservices;
        private readonly ILogger<BookController> _logger;
        public BookController(Bookservices bookservices, ILogger<BookController> logger)
        {
            _bookservices = bookservices;
            _logger = logger;
        }

        [HttpGet(Name = "Index")]
        public IActionResult Index()
        {
            IActionResult response = BadRequest();
            _logger.LogInformation("Entered the index");
            try
            {
                var GuId = Guid.NewGuid();
                _logger.LogInformation("Existed the index");
                response = Ok(new Response
                {
                    StatusCode = 0,
                    Message = "Index called successfully ",
                    Data = GuId,
                });
                return response;
            }
            catch (Exception ex)
            {
                _logger.LogError("Error: " + ex.Message);
                response = Ok(new Response
                {
                    StatusCode = 0,
                    Message = "Unknown error ocured.",
                    Data = string.Empty,
                });
                return response;
            }
        }

        [HttpGet("getBooks")]
        public async Task<IActionResult> getBooks(string formType)
        {
            IActionResult response = BadRequest();
            _logger.LogInformation("getBooks Of the BookController");
            try
            {
                IEnumerable<getBookDTO> result = await _bookservices.getBooks(formType);
                if (result == null)
                {
                    response = Ok(new Response
                    {
                        StatusCode = 1,
                        Message = "getBooks unsuccessfully ",
                    });
                }
                _logger.LogInformation("Exited getBooks Of the BookController with result " + result);

                response = Ok(new Response
                {
                    StatusCode = 0,
                    Message = "getBooks successfully ",
                    Data = result
                });
                return response;
            }
            catch (Exception ex)
            {
                _logger.LogError("Error Occured : " + ex.Message);
                response = Ok(new Response
                {
                    StatusCode = 0,
                    Message = "Unknown error ocured.",
                    Data = string.Empty,
                });
                return response;
            }
        }

        [HttpGet("getBookById")]
        public async Task<IActionResult> getBookById(Guid id)
        {
            IActionResult response = BadRequest();
            _logger.LogInformation("getBookBYId Of the BookController with id is " + id);

            try
            {
                getBookDTO result = await _bookservices.getBookById(id);
                if (result == null)
                {
                    _logger.LogError("No Book Found check the book Id");
                    response = Ok(new Response
                    {
                        StatusCode = 1,
                        Message = "No Book Found check the book Id",

                    });
                }
                _logger.LogInformation("Exited the getBookById successfully ");
                response = Ok(new Response
                {
                    StatusCode = 0,
                    Message = "getBookById successfully ",
                    Data = result
                });
                return response;
            }
            catch (Exception ex)
            {
                _logger.LogError("Error Occured : " + ex.Message);
                response = Ok(new Response
                {
                    StatusCode = 0,
                    Message = "Unknown error ocured.",
                    Data = string.Empty,
                });
                return response;
            }
        }
        [HttpPost("editBook")]
        public async Task<IActionResult> editBook([FromForm] editBookDto editedBook)
        {
            IActionResult response = BadRequest();
            _logger.LogInformation("editBook Of the BookController with book id " + editedBook.Id);

            try
            {
                var result = await _bookservices.EditBook(editedBook.Id, editedBook);
                _logger.LogError("Exited the editBook successfully ");
                response = Ok(result);
                return response;
            }
            catch (Exception ex)
            {
                _logger.LogError("Error Occured : " + ex.Message);
                response = Ok(new Response
                {
                    StatusCode = 0,
                    Message = "Unknown error ocured.",
                    Data = string.Empty,
                });
                return response;
            }

        }
        [HttpPost("deleteBook")]
        public async Task<IActionResult> deleteBook([FromBody] deleteBookDto request)
        {
            IActionResult response = BadRequest();
            _logger.LogInformation("deleteBook from Bookcontroller with id is " + request.Id);

            try
            {
                Guid guid = Guid.Parse(request.Id);
                var res = await _bookservices.deleteBook(guid);
                _logger.LogError("Exited the deleteBook successfully ");
                response = Ok(res);
                return response;
            }
            catch (Exception ex)
            {
                _logger.LogError("Error Occured : " + ex.Message);
                response = Ok(new Response
                {
                    StatusCode = 0,
                    Message = "Unknown error ocured.",
                    Data = string.Empty,
                });
                return response;
            }
        }
        [HttpPost("createBook")]
        public async Task<IActionResult> createBook([FromForm] createBookDTO createbook)
        {
            IActionResult response = BadRequest();
            _logger.LogInformation("createBook of Bookcontroller with id is ");

            try
            {
                if (!ModelState.IsValid)
                {
                    _logger.LogError("Some field is empty");
                    return View(new Response
                    {
                        StatusCode = 1,
                        Message = "Some field is empty"
                    });
                }
                var res = await _bookservices.createBook(createbook);
                _logger.LogError("Exited the createBook successfully ");
                return Ok(res);
            }
            catch (Exception ex)
            {
                _logger.LogError("Error Occured : " + ex.Message);
                response = Ok(new Response
                {
                    StatusCode = 0,
                    Message = "Unknown error ocured.",
                    Data = string.Empty,
                });
                return response;
            }
        }
        [HttpGet("getData")]
        public async Task<IActionResult> getData()
        {

            IActionResult response = BadRequest();
            _logger.LogInformation("createBook of Bookcontroller with id is ");

            try
            {
                var res = await _bookservices.getData();
                return Ok(res);
            }
            catch (Exception ex)
            {
                _logger.LogError("Error Occured : " + ex.Message);
                response = Ok(new Response
                {
                    StatusCode = 0,
                    Message = "Unknown error ocured.",
                    Data = string.Empty,
                });
                return response;
            }
        }
        [HttpGet("filterData")]
        public async Task<IActionResult> FilterBook([FromQuery] filterBookDTO filterData)
        {
            IActionResult response = BadRequest();
            _logger.LogInformation("FilterBook of Bookcontroller with id is ");

            try
            {
                var books = await _bookservices.FilterBook(filterData);
                var booksCount = await _bookservices.getBookCount(filterData);
                if (booksCount == 0)
                {
                    response = Ok(new Response
                    {
                        StatusCode = 1,
                        Message = "No result found ",
                        Data = string.Empty
                    });
                    return response;

                }


                Console.WriteLine("Filter form executed successfully.");

                return Ok(new Response
                {
                    StatusCode = 0,
                    Message = "Books filtered successfully.",
                    Data = new
                    {
                        books,
                        count = booksCount
                    }
                });

            }
            catch (Exception ex)
            {
                _logger.LogError("Error Occured : " + ex.Message);
                response = Ok(new Response
                {
                    StatusCode = 0,
                    Message = "Unknown error ocured.",
                    Data = string.Empty,
                });
                return response;
            }
        }
        
    }

}