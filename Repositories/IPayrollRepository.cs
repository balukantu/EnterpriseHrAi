using HrAi.Api.Models;

namespace HrAi.Api.Repositories;

public interface IPayrollRepository
{
    Task<Payroll?> GetLatestPayrollAsync(int employeeId);

    Task<Payroll?> GetPayrollByMonthAsync(int employeeId, int year, int month);
}