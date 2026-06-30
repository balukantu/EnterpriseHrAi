using HrAi.Api.Dtos;

namespace HrAi.Api.Services;

public interface IEmployeeService
{
    Task<EmployeeDto?> GetEmployeeAsync(int employeeId);
    Task<List<EmployeeDto>> GetEmployeesAsync();
}