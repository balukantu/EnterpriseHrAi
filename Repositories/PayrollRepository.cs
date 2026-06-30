using HrAi.Api.Data;
using HrAi.Api.Models;
using Microsoft.EntityFrameworkCore;

namespace HrAi.Api.Repositories;

public class PayrollRepository : IPayrollRepository
{
    private readonly HrAiDbContext _context;

    public PayrollRepository(HrAiDbContext context)
    {
        _context = context;
    }

    public async Task<Payroll?> GetLatestPayrollAsync(int employeeId)
    {
        return await _context.Payrolls
            .Where(x => x.EmployeeId == employeeId)
            .OrderByDescending(x => x.PayMonth)
            .FirstOrDefaultAsync();
    }

    public async Task<Payroll?> GetPayrollByMonthAsync(int employeeId, int year, int month)
    {
        return await _context.Payrolls
            .FirstOrDefaultAsync(x =>
                x.EmployeeId == employeeId &&
                x.PayMonth.Year == year &&
                x.PayMonth.Month == month);
    }
}