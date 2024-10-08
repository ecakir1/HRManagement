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
        private readonly IHttpClientFactory _httpClientFactory;

        public AdminController(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        public async Task<IActionResult> Index()
        {
            var client = _httpClientFactory.CreateClient("HRClient");
            var response = await client.GetAsync("https://localhost:7249/api/Admin/InactiveUsers");

            if (response.IsSuccessStatusCode)
            {
                var users = await response.Content.ReadFromJsonAsync<List<Employee>>();
                return View(users);
            }

            return View(new List<Employee>());
        }

        [HttpPost]
        public async Task<IActionResult> Approve(string userId)
        {
            var client = _httpClientFactory.CreateClient("HRClient");
            var response = await client.PostAsync($"https://localhost:7249/api/Admin/Approve/{userId}", null);

            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<IActionResult> Reject(string userId)
        {
            var client = _httpClientFactory.CreateClient("HRClient");
            var response = await client.PostAsync($"https://localhost:7249/api/Admin/Reject/{userId}", null);

            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<IActionResult> AssignCompanyExecutive(string userId)
        {
            var client = _httpClientFactory.CreateClient("HRClient");
            var response = await client.PostAsync($"https://localhost:7249/api/Admin/AssignCompanyExecutive/{userId}", null);

            return RedirectToAction("Index");
        }

        public IActionResult Requests()
        {
            return View();
        }
    }
}
