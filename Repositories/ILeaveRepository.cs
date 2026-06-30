using HrAi.Api.Models;

namespace HrAi.Api.Repositories;

public interface ILeaveRepository
{
    Task<List<LeaveBalance>> GetLeaveBalancesAsync(int employeeId);
    Task<LeaveBalance?> GetLeaveBalanceAsync(int employeeId, string leaveType);
    Task AddLeaveRequestAsync(LeaveRequest request);
    Task SaveChangesAsync();
}