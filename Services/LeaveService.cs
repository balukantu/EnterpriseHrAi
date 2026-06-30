using HrAi.Api.Dtos;
using HrAi.Api.Models;
using HrAi.Api.Repositories;

namespace HrAi.Api.Services;

public class LeaveService : ILeaveService
{
    private readonly ILeaveRepository _leaveRepository;

    public LeaveService(ILeaveRepository leaveRepository)
    {
        _leaveRepository = leaveRepository;
    }

    public async Task<List<LeaveBalance>> GetLeaveBalancesAsync(int employeeId)
    {
        return await _leaveRepository.GetLeaveBalancesAsync(employeeId);
    }

    public async Task<string> ApplyLeaveAsync(CreateLeaveRequestDto request)
    {
        if (request.ToDate < request.FromDate)
            return "ToDate cannot be earlier than FromDate.";

        int totalDays = (request.ToDate.Date - request.FromDate.Date).Days + 1;

        var balance = await _leaveRepository.GetLeaveBalanceAsync(
            request.EmployeeId,
            request.LeaveType);

        if (balance == null)
            return $"No leave balance found for leave type: {request.LeaveType}.";

        if (balance.BalanceDays < totalDays)
            return $"Insufficient leave balance. Available: {balance.BalanceDays}, Requested: {totalDays}.";

        var leaveRequest = new LeaveRequest
        {
            EmployeeId = request.EmployeeId,
            LeaveType = request.LeaveType,
            FromDate = request.FromDate,
            ToDate = request.ToDate,
            TotalDays = totalDays,
            Status = "Pending",
            Reason = request.Reason,
            CreatedAt = DateTime.UtcNow
        };

        balance.BalanceDays -= totalDays;

        await _leaveRepository.AddLeaveRequestAsync(leaveRequest);
        await _leaveRepository.SaveChangesAsync();

        return $"Leave request submitted successfully. Requested days: {totalDays}. Remaining balance: {balance.BalanceDays}.";
    }
}