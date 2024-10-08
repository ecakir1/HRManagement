using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using API.Models;
using API.Services;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly IAuthService _authService;

        public AccountController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginModel model)
        {
            var token = await _authService.LoginAsync(model);
            if (token != null)
            {
                return Ok(new { Token = token });
            }
            return Unauthorized();
        }

        [HttpPost("SignUp")]
        public async Task<IActionResult> SignUp([FromBody] SignUpModel model)
        {
            var result = await _authService.SignUpAsync(model);
            if (result)
            {
                return Ok();
            }
            return BadRequest();
        }
    }
}
