using DAL.Core.IConfiguration;
using DAL.Models;
using Microsoft.AspNetCore.Identity;

namespace API.EmployeeServices
{
    public class EmployeeService : IEmployeeService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly UserManager<Employee> _userManager;
        private readonly SignInManager<Employee> _signInManager;

        public EmployeeService(IUnitOfWork unitOfWork, UserManager<Employee> userManager, SignInManager<Employee> signInManager)
        {
            _unitOfWork = unitOfWork;
            _userManager = userManager;
            _signInManager = signInManager;
        }

        public async Task<IdentityResult> CreateEmployee(Employee newEmployee, EmployeeDetail newEmployeeDetail, string password)
        {
            // Check if a user with the same email already exists
            var existingUser = await _userManager.FindByEmailAsync(newEmployee.Email);
            if (existingUser != null)
            {
                return IdentityResult.Failed(new IdentityError { Description = "Email is already in use." });
            }

            // Create the user using UserManager
            var result = await _userManager.CreateAsync(newEmployee, password);
            if (result.Succeeded)
            {
                // Add employee details
                newEmployeeDetail.EmployeeId = newEmployee.Id;
                await _unitOfWork.Employees.AddDetailAsync(newEmployeeDetail);
                await _unitOfWork.CommitAsync();
            }

            return result;
        }

        public async Task<IEnumerable<Employee>> GetAllEmployees()
        {
            return await _unitOfWork.Employees.GetAllAsync();
        }

        public async Task<Employee> GetEmployeeById(Guid id)
        {
            return await _unitOfWork.Employees.GetByIdAsync(id);
        }

        public async Task UpdateEmployee(Employee employeeToBeUpdated, Employee employee)
        {
            employeeToBeUpdated.UserName = employee.UserName;
            employeeToBeUpdated.Email = employee.Email;
            employeeToBeUpdated.PhoneNumber = employee.PhoneNumber;
            employeeToBeUpdated.IsActive = employee.IsActive;
            employeeToBeUpdated.Updated_At = DateTime.UtcNow;

            await _unitOfWork.CommitAsync();
        }

        public async Task UpdateEmployeeDetail(EmployeeDetail employeeDetailToBeUpdated, EmployeeDetail employeeDetail)
        {
            employeeDetailToBeUpdated.Address = employeeDetail.Address;
            employeeDetailToBeUpdated.Position = employeeDetail.Position;
            employeeDetailToBeUpdated.Department = employeeDetail.Department;
            employeeDetailToBeUpdated.City = employeeDetail.City;
            employeeDetailToBeUpdated.Educations = employeeDetail.Educations;
            employeeDetailToBeUpdated.Certifications = employeeDetail.Certifications;
            employeeDetailToBeUpdated.Experiences = employeeDetail.Experiences;
            employeeDetailToBeUpdated.RemainingLeaveDays = employeeDetail.RemainingLeaveDays;

            await _unitOfWork.CommitAsync();
        }

        public async Task DeleteEmployee(Employee employee)
        {
            _unitOfWork.Employees.Remove(employee);
            await _unitOfWork.CommitAsync();
        }

        // Yeni eklenen kullanıcı doğrulama metodu
        public async Task<bool> ValidateUserAsync(string username, string password)
        {
            var user = await _userManager.FindByNameAsync(username);
            if (user != null)
            {
                var result = await _signInManager.CheckPasswordSignInAsync(user, password, false);
                return result.Succeeded;
            }
            return false;
        }

        // Yeni eklenen kullanıcı onaylama metodu
        public async Task<bool> ApproveUserAsync(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user != null)
            {
                user.IsActive = true;
                await _userManager.UpdateAsync(user);
                await _unitOfWork.CommitAsync();
                return true;
            }
            return false;
        }

        // Yeni eklenen kullanıcı reddetme metodu
        public async Task<bool> RejectUserAsync(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user != null)
            {
                user.IsActive = false;
                await _userManager.UpdateAsync(user);
                await _unitOfWork.CommitAsync();
                return true;
            }
            return false;
        }

        // Yeni eklenen şirket yöneticisi atama metodu
        public async Task<bool> AssignCompanyExecutiveAsync(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user != null)
            {
                // Şirket yöneticisi rolünü atama
                var result = await _userManager.AddToRoleAsync(user, "CompanyExecutive");
                if (result.Succeeded)
                {
                    await _unitOfWork.CommitAsync();
                    return true;
                }
            }
            return false;
        }
    }
}
