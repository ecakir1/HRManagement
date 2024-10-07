using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;
using System.Linq;
using API.Models;
using DAL.Models; // Kullanıcı modelinin namespace'ini ekleyin

namespace MVC.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdminController : Controller
    {
        private readonly UserManager<Employee> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public AdminController(UserManager<Employee> userManager, RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _roleManager = roleManager;
        }

        public IActionResult Index()
        {
            var users = _userManager.Users.Where(u => !u.IsActive).ToList();
            return View(users);
        }

        [HttpPost]
        public async Task<IActionResult> Approve(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user != null)
            {
                user.IsActive = true;
                await _userManager.UpdateAsync(user);
            }
            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<IActionResult> Reject(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user != null)
            {
                await _userManager.DeleteAsync(user);
            }
            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<IActionResult> AssignCompanyExecutive(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user != null)
            {
                var roleExists = await _roleManager.RoleExistsAsync("CompanyExecutive");
                if (!roleExists)
                {
                    await _roleManager.CreateAsync(new IdentityRole("CompanyExecutive"));
                }
                await _userManager.AddToRoleAsync(user, "CompanyExecutive");
            }
            return RedirectToAction("Index");
        }

        public IActionResult Requests()
        {
            return View();
        }
    }
}
