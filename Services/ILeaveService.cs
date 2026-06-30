using HrAi.Api.Dtos;
using HrAi.Api.Models;

namespace HrAi.Api.Services;

public interface ILeaveService
{
    Task<List<LeaveBalance>> GetLeaveBalancesAsync(int employeeId);
    Task<string> ApplyLeaveAsync(CreateLeaveRequestDto request);
}