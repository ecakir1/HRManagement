using System.Threading.Tasks;
using API.Models;
using Microsoft.AspNetCore.Identity;
using DAL.Models;

namespace API.Services
{
    public class AuthService : IAuthService
    {
        private readonly UserManager<Employee> _userManager;
        private readonly SignInManager<Employee> _signInManager;
        private readonly TokenService _tokenService;

        public AuthService(UserManager<Employee> userManager, SignInManager<Employee> signInManager, TokenService tokenService)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _tokenService = tokenService;
        }

        public async Task<string> LoginAsync(LoginModel model)
        {
            var user = await _userManager.FindByNameAsync(model.Username);
            if (user != null)
            {
                var result = await _signInManager.CheckPasswordSignInAsync(user, model.Password, false);
                if (result.Succeeded)
                {
                    return _tokenService.GenerateToken(user);
                }
            }
            return null;
        }

        public async Task<bool> SignUpAsync(SignUpModel model)
        {
            var user = new Employee
            {
                UserName = model.Username,
                Email = model.Email
            };

            var result = await _userManager.CreateAsync(user, model.Password);
            return result.Succeeded;
        }
    }
}
