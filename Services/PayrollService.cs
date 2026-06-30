using HrAi.Api.Dtos;
using HrAi.Api.Repositories;

namespace HrAi.Api.Services;

public class PayrollService : IPayrollService
{
    private readonly IPayrollRepository _payrollRepository;

    public PayrollService(IPayrollRepository payrollRepository)
    {
        _payrollRepository = payrollRepository;
    }

    public async Task<PayrollDto?> GetLatestPayrollAsync(int employeeId)
    {
        var payroll = await _payrollRepository.GetLatestPayrollAsync(employeeId);

        if (payroll == null)
            return null;

        return new PayrollDto
        {
            PayrollId = payroll.PayrollId,
            EmployeeId = payroll.EmployeeId,
            PayMonth = payroll.PayMonth,
            GrossSalary = payroll.GrossSalary,
            TaxAmount = payroll.TaxAmount,
            NetSalary = payroll.NetSalary
        };
    }

    public async Task<PayrollDto?> GetPayrollByMonthAsync(int employeeId, int year, int month)
    {
        var payroll = await _payrollRepository.GetPayrollByMonthAsync(employeeId, year, month);

        if (payroll == null)
            return null;

        return new PayrollDto
        {
            PayrollId = payroll.PayrollId,
            EmployeeId = payroll.EmployeeId,
            PayMonth = payroll.PayMonth,
            GrossSalary = payroll.GrossSalary,
            TaxAmount = payroll.TaxAmount,
            NetSalary = payroll.NetSalary
        };
    }
}