using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using API.EmployeeServices;
using API.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Text;
using System;
using System.Security.Claims;
using DAL.Models;

namespace API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly EmployeeService _employeeService;
        private readonly IConfiguration _configuration;

        public AuthController(EmployeeService employeeService, IConfiguration configuration)
        {
            _employeeService = employeeService;
            _configuration = configuration;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginModel model)
        {
            if (await _employeeService.ValidateUserAsync(model.Username, model.Password))
            {
                var token = GenerateJwtToken(model.Username);
                return Ok(new { token });
            }
            return Unauthorized("Invalid credentials");
        }

        private string GenerateJwtToken(string username)
        {
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, username),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };

            var token = new JwtSecurityToken(
                issuer: _configuration["Jwt:Issuer"],
                audience: _configuration["Jwt:Audience"],
                claims: claims,
                expires: DateTime.Now.AddMinutes(30),
                signingCredentials: creds);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        [HttpPost("signup")]
        public async Task<IActionResult> SignUp([FromBody] SignUpModel model)
        {
            var user = new Employee
            {
                UserName = model.Username,
                Email = model.Email
            };

            var result = await _employeeService.CreateEmployee(user, new EmployeeDetail(), model.Password);

            if (result != null)
            {
                return Ok(new { message = "User created successfully" });
            }

            return BadRequest("User registration failed");
        }

    }
}
