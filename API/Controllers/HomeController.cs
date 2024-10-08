using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HomeController : ControllerBase
    {
        [HttpGet("Privacy")]
        public IActionResult Privacy()
        {
            return Ok("Privacy Policy");
        }
    }
}
