using DAL.Models;

namespace DAL.Core.IRepository
{
    public interface IEmployeeRepository : IGenericRepository<Employee>
    {
        Task AddAsync(Employee employee);

        Task AddDetailAsync(EmployeeDetail employeeDetail);
    }
}
