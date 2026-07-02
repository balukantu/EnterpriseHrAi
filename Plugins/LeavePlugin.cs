using HrAi.Api.Dtos;
using HrAi.Api.Models;
using HrAi.Api.Services;
using Microsoft.SemanticKernel;
using System.ComponentModel;

namespace HrAi.Api.Plugins;

public class LeavePlugin
{
    private readonly ILeaveService _leaveService;
    private readonly IAiPluginExecutionLogService _pluginLogService;

    public LeavePlugin(ILeaveService leaveService, IAiPluginExecutionLogService pluginLogService)
    {
        _leaveService = leaveService;
        _pluginLogService = pluginLogService;
    }

    [KernelFunction]
    [Description("Use this function when an employee asks for available leave balance, PTO balance, vacation balance, sick leave balance, remaining leave days, or whether they have enough leave.")]
    public async Task<string> GetLeaveBalanceAsync(
        [Description("The employee ID of the logged-in employee.")] int employeeId)
    {
        var stopwatch = System.Diagnostics.Stopwatch.StartNew();

        var balances = await _leaveService.GetLeaveBalancesAsync(employeeId);

        if (balances == null || balances.Count == 0)
            return $"No leave balance found for employee id {employeeId}.";

        stopwatch.Stop();

        var result = string.Join(Environment.NewLine,
            balances.Select(x => $"{x.LeaveType}: {x.BalanceDays} days"));

        await _pluginLogService.SaveAsync(new AiPluginExecutionLog
        {
            PluginExecutionLogId = Guid.NewGuid(),
            EmployeeId = employeeId,
            PluginName = "LeavePlugin",
            FunctionName = "GetLeaveBalanceAsync",
            InputJson = $"{{\"employeeId\":{employeeId}}}",
            OutputText = result,
            DurationMs = (int)stopwatch.ElapsedMilliseconds,
            Status = "Success",
            CreatedAt = DateTime.UtcNow
        });

        return result;
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