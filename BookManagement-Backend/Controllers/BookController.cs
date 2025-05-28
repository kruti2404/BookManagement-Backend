using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Services;
using DTOs;
using Models.Shared;
using System;
namespace BookManagement_Backend.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class BookController : Controller
    {

        private readonly Bookservices _bookservices;
        public BookController(Bookservices bookservices)
        {
            _bookservices = bookservices;
        }

        [HttpGet(Name = "Index")]
        public IActionResult Index()
        {
            var response = Unauthorized();
            var GuId = Guid.NewGuid();

            Console.WriteLine("GuId is ", GuId);
            Console.WriteLine("The type of GuId is ", typeof(Guid));

            return Ok(new Response
            {
                StatusCode = 0,
                Message = "Index called successfully ",
                Data = GuId,
            });
        }

        [HttpGet("getBooks")]
        public async Task<IActionResult> getBooks(string formType)
        {
            Console.WriteLine("getBooks Of the BookController");
            IEnumerable<getBookDTO> result = await _bookservices.getBooks(formType);
            if (result == null)
            {
                return Ok(new Response
                {
                    StatusCode = 1,
                    Message = "getBooks unsuccessfully ",
                });
            }
            Console.WriteLine("The result is " + result);


            return Json(new Response
            {
                StatusCode = 0,
                Message = "getBooks successfully ",
                Data = result
            });
        }

        [HttpGet("getBookById")]
        public async Task<IActionResult> getBookById(Guid id)
        {
            Console.WriteLine("Id is " + id);
            Console.WriteLine("Get by id is ");
            getBookDTO result = await _bookservices.getBookById(id);
            if (result == null)
            {
                return Ok(new Response
                {
                    StatusCode = 1,
                    Message = "No Book Found check the book Id",

                });
            }
            return Json(new Response
            {
                StatusCode = 0,
                Message = "getBookById successfully ",
                Data = result
            });
        }
        [HttpPost("editBook")]
        public async Task<IActionResult> editBook([FromBody] editBookDto editedBook)
        {
            try
            {
                Console.WriteLine("The edit is called ");
                Console.WriteLine("Author name is " + editedBook.AuthorName);
                var result = await _bookservices.EditBook(editedBook.Id, editedBook);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return Ok(new Response
                {
                    StatusCode = 1,
                    Message = ex.Message
                });
            }
        }

        [HttpPost("deleteBook")]
        public async Task<IActionResult> deleteBook(Guid id)
        {
            try
            {
                var response = await _bookservices.deleteBook(id);
                return Ok(response);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpPost("createBook")]
        public async Task<IActionResult> createBook(createBookDTO createbook)
        {
            try
            {
                Console.WriteLine("createBook of Bookcontroller executing ");
                if (!ModelState.IsValid)
                {
                    return View(new Response
                    {
                        StatusCode = 1,
                        Message = "Some field is empty"
                    });
                }
                var response =await _bookservices.createBook(createbook);

                return Ok(response);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
