using System.Threading.Tasks;
using DTOs;
using Microsoft.AspNetCore.Mvc;
using Models.Shared;
using Services;

namespace BookManagement_Backend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SpBookController : Controller
    {
        private readonly SpBookService _bookservices;
        private readonly ILogger<BookController> _logger;
        public SpBookController(SpBookService bookservices, ILogger<BookController> logger)
        {
            _bookservices = bookservices;
            _logger = logger;
        }
        [HttpGet("filterData")]
        public async Task<IActionResult> filterData([FromQuery] filterBookDTO filterBook)
        {

            IActionResult response = BadRequest();
            _logger.LogInformation("Entered the FilterData Method of Strored Procedure");

            try
            {
                var result = await _bookservices.filterData(filterBook);
                response = Ok(new Response
                {
                    StatusCode = 0,
                    Message = "FilterData called successfully ",
                    Data = new
                    {
                        result.books,
                        result.Totalcount
                    }
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
        [HttpPost("createOrEdit")]
        public async Task<IActionResult> createOrEdit([FromForm] createOrEditBook createOrEditBook)
        {

            IActionResult response = BadRequest();
            _logger.LogInformation("Entered the createOrEdit Method of Strored Procedure");

            try
            {
                var result = await _bookservices.createOrEdit(createOrEditBook);
                response = Ok(result);
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
    }
}
