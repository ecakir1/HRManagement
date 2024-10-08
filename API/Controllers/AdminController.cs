using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using DAL.Models;
using DAL.Services;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "Admin")]
    public class AdminController : ControllerBase
    {
        private readonly IEmployeeService _employeeService;

        public AdminController(IEmployeeService employeeService)
        {
            _employeeService = employeeService;
        }

        [HttpGet("InactiveUsers")]
        public async Task<ActionResult<List<Employee>>> GetInactiveUsers()
        {
            var users = await _employeeService.GetInactiveUsersAsync();
            return Ok(users);
        }

        [HttpPost("Approve/{userId}")]
        public async Task<IActionResult> ApproveUser(string userId)
        {
            var result = await _employeeService.ApproveUserAsync(userId);
            if (result)
            {
                return Ok();
            }
            return BadRequest();
        }

        [HttpPost("Reject/{userId}")]
        public async Task<IActionResult> RejectUser(string userId)
        {
            var result = await _employeeService.RejectUserAsync(userId);
            if (result)
            {
                return Ok();
            }
            return BadRequest();
        }

        [HttpPost("AssignCompanyExecutive/{userId}")]
        public async Task<IActionResult> AssignCompanyExecutive(string userId)
        {
            var result = await _employeeService.AssignCompanyExecutiveAsync(userId);
            if (result)
            {
                return Ok();
            }
            return BadRequest();
        }
    }
}
