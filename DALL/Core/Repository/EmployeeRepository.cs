using DAL.Core.IRepository;
using DAL.Models;

namespace DAL.Core.Repository
{
    public class EmployeeRepository : GenericRepository<Employee>, IEmployeeRepository
    {
        public EmployeeRepository(HRManagementDbContext context) : base(context) { }

        public Task AddAsync(Employee employee)
        {
            PersonnelManagementDBContext.Employees.Add(employee);
            return Task.CompletedTask;
            
        }

        public Task AddDetailAsync(EmployeeDetail employeeDetail)
        {
            PersonnelManagementDBContext.EmployeeDetails.Add(employeeDetail);
            return Task.CompletedTask;

        }

        private HRManagementDbContext PersonnelManagementDBContext { get { return Context; } }


    }
}
