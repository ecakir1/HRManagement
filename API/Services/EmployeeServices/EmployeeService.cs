using DAL.Core.IConfiguration;
using DAL.Models;

namespace API.EmployeeServices
{
    public class EmployeeService
    {
        private readonly IUnitOfWork unitOfWork;

        public EmployeeService(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }

        public async Task<Employee> CreateEmployee(Employee newEmployee, EmployeeDetail newEmployeeDetail)
        {
            await unitOfWork.Employees.AddAsync(newEmployee);
            newEmployeeDetail.EmployeeId = newEmployee.Id;
            await unitOfWork.Employees.AddDetailAsync(newEmployeeDetail);
            await unitOfWork.CommitAsync();

            return newEmployee;
        }

        public async Task<IEnumerable<Employee>> GetAllEmployees()
        {
            return await unitOfWork.Employees.GetAllAsync();
        }

        public async Task<Employee> GetEmployeeById(Guid id)
        {
            return await unitOfWork.Employees.GetByIdAsync(id);
        }

        public async Task UpdateEmployee(Employee employeeToBeUpdated, Employee employee)
        {
            employeeToBeUpdated.UserName = employee.UserName;
            employeeToBeUpdated.Email = employee.Email;
            employeeToBeUpdated.PhoneNumber = employee.PhoneNumber;
            employeeToBeUpdated.IsActive = employee.IsActive;
            employeeToBeUpdated.Updated_At = DateTime.UtcNow;

            await unitOfWork.CommitAsync();
        }

        public async Task UpdateEmployeeDetail(EmployeeDetail employeeDetailToBeUpdated, EmployeeDetail employeeDetail)
        {
            employeeDetailToBeUpdated.Address = employeeDetail.Address;
            employeeDetailToBeUpdated.Position = employeeDetail.Position;
            employeeDetailToBeUpdated.Department = employeeDetail.Department;
            employeeDetailToBeUpdated.City = employeeDetail.City;
            employeeDetailToBeUpdated.Education = employeeDetail.Education;
            employeeDetailToBeUpdated.Certifications = employeeDetail.Certifications;
            employeeDetailToBeUpdated.Experience = employeeDetail.Experience;
            employeeDetailToBeUpdated.RemainingLeaveDays = employeeDetail.RemainingLeaveDays;

            await unitOfWork.CommitAsync();
        }

        public async Task DeleteEmployee(Employee employee)
        {
            unitOfWork.Employees.Remove(employee);
            await unitOfWork.CommitAsync();
        }
    }
}
