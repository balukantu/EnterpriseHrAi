using HrAi.Api.Dtos;

namespace HrAi.Api.Services;

public interface IPayrollService
{
    Task<PayrollDto?> GetLatestPayrollAsync(int employeeId);

    Task<PayrollDto?> GetPayrollByMonthAsync(int employeeId, int year, int month);
}