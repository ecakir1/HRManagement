using System.Collections.Generic;
using System.Threading.Tasks;
using DAL.Models;

namespace API.IServices
{
    public interface IEmployeeService
    {
        Task<List<Employee>> GetInactiveUsersAsync();
        Task<bool> ApproveUserAsync(string userId);
        Task<bool> RejectUserAsync(string userId);
        Task<bool> AssignCompanyExecutiveAsync(string userId);
    }
}
