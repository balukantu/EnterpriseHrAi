using HrAi.Api.Dtos;

namespace HrAi.Api.Services;

public interface IEmployeeProfileService
{
    Task<EmployeeProfileDto?> GetMyProfileAsync(int employeeId);

    Task<string> GetMyManagerAsync(int employeeId);
}