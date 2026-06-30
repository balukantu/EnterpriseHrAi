using HrAi.Api.Data;
using HrAi.Api.Models;
using Microsoft.EntityFrameworkCore;

namespace HrAi.Api.Repositories;

public class LeaveRepository : ILeaveRepository
{
    private readonly HrAiDbContext _context;

    public LeaveRepository(HrAiDbContext context)
    {
        _context = context;
    }

    public async Task<List<LeaveBalance>> GetLeaveBalancesAsync(int employeeId)
    {
        return await _context.LeaveBalances
            .Where(x => x.EmployeeId == employeeId)
            .ToListAsync();
    }

    public async Task<LeaveBalance?> GetLeaveBalanceAsync(int employeeId, string leaveType)
    {
        return await _context.LeaveBalances
            .FirstOrDefaultAsync(x =>
                x.EmployeeId == employeeId &&
                x.LeaveType == leaveType);
    }

    public async Task AddLeaveRequestAsync(LeaveRequest request)
    {
        await _context.LeaveRequests.AddAsync(request);
    }

    public async Task SaveChangesAsync()
    {
        await _context.SaveChangesAsync();
    }
}