using HrAi.Api.Dtos;
using HrAi.Api.Repositories;

namespace HrAi.Api.Services;

public class EmployeeService : IEmployeeService
{
    private readonly IEmployeeRepository _employeeRepository;

    public EmployeeService(IEmployeeRepository employeeRepository)
    {
        _employeeRepository = employeeRepository;
    }

    public async Task<EmployeeDto?> GetEmployeeAsync(int employeeId)
    {
        var employee = await _employeeRepository.GetByIdAsync(employeeId);

        if (employee == null)
            return null;

        return new EmployeeDto
        {
            EmployeeId = employee.EmployeeId,
            FullName = $"{employee.FirstName} {employee.LastName}",
            Email = employee.Email,
            Department = employee.Department?.Name ?? "",
            ManagerName = employee.Manager == null
                ? null
                : $"{employee.Manager.FirstName} {employee.Manager.LastName}",
            HireDate = employee.HireDate
        };
    }

    public async Task<List<EmployeeDto>> GetEmployeesAsync()
    {
        var employees = await _employeeRepository.GetAllAsync();

        return employees.Select(employee => new EmployeeDto
        {
            EmployeeId = employee.EmployeeId,
            FullName = $"{employee.FirstName} {employee.LastName}",
            Email = employee.Email,
            Department = employee.Department?.Name ?? "",
            ManagerName = employee.Manager == null
                ? null
                : $"{employee.Manager.FirstName} {employee.Manager.LastName}",
            HireDate = employee.HireDate
        }).ToList();
    }
}