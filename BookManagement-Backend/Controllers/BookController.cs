using Microsoft.AspNetCore.Mvc;

namespace BookManagement_Backend.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class BookController : Controller
    {
        [HttpGet(Name = "Index")]
        public IActionResult Index()
        {
            var data = new
            {
                status = 200,
                message = "Index called successfully ",
            };

            return Ok(data);
        }
    }
}
