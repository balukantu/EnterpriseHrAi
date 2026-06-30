using System.ComponentModel;
using HrAi.Api.Services;
using Microsoft.SemanticKernel;

namespace HrAi.Api.Plugins;

public class PayrollPlugin
{
    private readonly IPayrollService _payrollService;

    public PayrollPlugin(IPayrollService payrollService)
    {
        _payrollService = payrollService;
    }

    [KernelFunction]
    [Description("Use this function when the logged-in employee asks for their latest payroll, latest salary, recent salary, current salary, net pay, gross pay, tax amount, or payslip summary.")]
    public async Task<string> GetLatestPayrollAsync(
        [Description("Logged-in employee ID.")] int employeeId)
    {
        var payroll = await _payrollService.GetLatestPayrollAsync(employeeId);

        if (payroll == null)
            return "No payroll record found for this employee.";

        return $"""
        Latest Payroll:
        Month: {payroll.PayMonth:MMMM yyyy}
        Gross Salary: {payroll.GrossSalary:C}
        Tax Amount: {payroll.TaxAmount:C}
        Net Salary: {payroll.NetSalary:C}
        """;
    }

    [KernelFunction]
    [Description("Use this function when the logged-in employee asks for payroll, salary, net pay, gross pay, tax, or payslip details for a specific month and year.")]
    public async Task<string> GetPayrollByMonthAsync(
        [Description("Logged-in employee ID.")] int employeeId,
        [Description("Payroll year. Example: 2026.")] int year,
        [Description("Payroll month number. Example: 6 for June.")] int month)
    {
        var payroll = await _payrollService.GetPayrollByMonthAsync(employeeId, year, month);

        if (payroll == null)
            return $"No payroll record found for {month}/{year}.";

        return $"""
        Payroll for {payroll.PayMonth:MMMM yyyy}:
        Gross Salary: {payroll.GrossSalary:C}
        Tax Amount: {payroll.TaxAmount:C}
        Net Salary: {payroll.NetSalary:C}
        """;
    }
}