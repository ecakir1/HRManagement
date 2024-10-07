using DAL.Core.IConfiguration;
using DAL.Models;
using Microsoft.AspNetCore.Identity;

namespace API.EmployeeServices
{
    public class EmployeeService
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly UserManager<Employee> userManager;
        private readonly SignInManager<Employee> signInManager;

        public EmployeeService(IUnitOfWork unitOfWork, UserManager<Employee> userManager, SignInManager<Employee> signInManager)
        {
            this.unitOfWork = unitOfWork;
            this.userManager = userManager;
            this.signInManager = signInManager;
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
            employeeDetailToBeUpdated.Educations = employeeDetail.Educations;
            employeeDetailToBeUpdated.Certifications = employeeDetail.Certifications;
            employeeDetailToBeUpdated.Experiences= employeeDetail.Experiences;
            employeeDetailToBeUpdated.RemainingLeaveDays = employeeDetail.RemainingLeaveDays;

            await unitOfWork.CommitAsync();
        }

        public async Task DeleteEmployee(Employee employee)
        {
            unitOfWork.Employees.Remove(employee);
            await unitOfWork.CommitAsync();
        }

        // Yeni eklenen kullanıcı doğrulama metodu
        public async Task<bool> ValidateUserAsync(string username, string password)
        {
            var user = await userManager.FindByNameAsync(username);
            if (user != null)
            {
                var result = await signInManager.CheckPasswordSignInAsync(user, password, false);
                return result.Succeeded;
            }
            return false;
        }
    }
}
