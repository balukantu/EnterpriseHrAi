using HrAi.Api.Models;

namespace HrAi.Api.Repositories;

public interface IEmployeeRepository
{
    Task<Employee?> GetByIdAsync(int employeeId);
    Task<List<Employee>> GetAllAsync();
}