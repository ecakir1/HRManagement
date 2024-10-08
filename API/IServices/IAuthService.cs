using System.Threading.Tasks;
using API.Models;

namespace API.Services
{
    public interface IAuthService
    {
        Task<string> LoginAsync(LoginModel model);
        Task<bool> SignUpAsync(SignUpModel model);
    }
}
