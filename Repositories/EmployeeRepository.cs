using HrAi.Api.Data;
using HrAi.Api.Models;
using Microsoft.EntityFrameworkCore;

namespace HrAi.Api.Repositories;

public class EmployeeRepository : IEmployeeRepository
{
    private readonly HrAiDbContext _context;

    public EmployeeRepository(HrAiDbContext context)
    {
        _context = context;
    }

    public async Task<Employee?> GetByIdAsync(int employeeId)
    {
        return await _context.Employees
            .Include(e => e.Department)
            .Include(e => e.Manager)
            .FirstOrDefaultAsync(e => e.EmployeeId == employeeId);
    }

    public async Task<List<Employee>> GetAllAsync()
    {
        return await _context.Employees
            .Include(e => e.Department)
            .Include(e => e.Manager)
            .ToListAsync();
    }
}