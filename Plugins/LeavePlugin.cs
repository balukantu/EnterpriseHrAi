using System.ComponentModel;
using HrAi.Api.Dtos;
using HrAi.Api.Services;
using Microsoft.SemanticKernel;

namespace HrAi.Api.Plugins;

public class LeavePlugin
{
    private readonly ILeaveService _leaveService;

    public LeavePlugin(ILeaveService leaveService)
    {
        _leaveService = leaveService;
    }

    [KernelFunction]
    [Description("Use this function when an employee asks for available leave balance, PTO balance, vacation balance, sick leave balance, remaining leave days, or whether they have enough leave.")]
    public async Task<string> GetLeaveBalanceAsync(
        [Description("The employee ID of the logged-in employee.")] int employeeId)
    {
        var balances = await _leaveService.GetLeaveBalancesAsync(employeeId);

        if (balances == null || balances.Count == 0)
            return $"No leave balance found for employee id {employeeId}.";

        return string.Join(Environment.NewLine,
            balances.Select(x => $"{x.LeaveType}: {x.BalanceDays} days"));
    }

    [KernelFunction]
    [Description("Use this function when an employee wants to apply, request, book, or schedule leave, vacation, PTO, sick leave, personal leave, or time off for specific dates.")]
    public async Task<string> ApplyLeaveAsync(
        [Description("The employee ID of the logged-in employee.")] int employeeId,
        [Description("Leave type. Example: Vacation, Sick, Personal.")] string leaveType,
        [Description("Leave start date in yyyy-MM-dd format.")] DateTime fromDate,
        [Description("Leave end date in yyyy-MM-dd format.")] DateTime toDate,
        [Description("Reason for leave request.")] string? reason)
    {
        var request = new CreateLeaveRequestDto
        {
            EmployeeId = employeeId,
            LeaveType = leaveType,
            FromDate = fromDate,
            ToDate = toDate,
            Reason = reason
        };

        return await _leaveService.ApplyLeaveAsync(request);
    }
}