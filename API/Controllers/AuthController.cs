using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using API.EmployeeServices;
using API.Models; // LoginModel namespace'i ekleyin

namespace API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly EmployeeService employeeService;

        public AuthController(EmployeeService employeeService)
        {
            this.employeeService = employeeService;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginModel model)
        {
            if (await employeeService.ValidateUserAsync(model.Username, model.Password))
            {
                return Ok("Login successful");
            }
            return Unauthorized("Invalid credentials");
        }
    }
}
