using HrAi.Api.Dtos;
using HrAi.Api.Repositories;

namespace HrAi.Api.Services;

public class EmployeeProfileService : IEmployeeProfileService
{
    private readonly IEmployeeRepository _employeeRepository;

    public EmployeeProfileService(IEmployeeRepository employeeRepository)
    {
        _employeeRepository = employeeRepository;
    }

    public async Task<EmployeeProfileDto?> GetMyProfileAsync(int employeeId)
    {
        var employee = await _employeeRepository.GetByIdAsync(employeeId);

        if (employee == null)
            return null;

        return new EmployeeProfileDto
        {
            EmployeeId = employee.EmployeeId,
            FullName = $"{employee.FirstName} {employee.LastName}",
            Email = employee.Email,
            Department = employee.Department?.Name ?? "",
            ManagerName = employee.Manager == null
                ? null
                : $"{employee.Manager.FirstName} {employee.Manager.LastName}",
            ManagerEmail = employee.Manager?.Email,
            HireDate = employee.HireDate
        };
    }

    public async Task<string> GetMyManagerAsync(int employeeId)
    {
        var employee = await _employeeRepository.GetByIdAsync(employeeId);

        if (employee == null)
            return "Employee not found.";

        if (employee.Manager == null)
            return "You do not have a manager assigned.";

        return $"Your manager is {employee.Manager.FirstName} {employee.Manager.LastName}. Email: {employee.Manager.Email}.";
    }
}