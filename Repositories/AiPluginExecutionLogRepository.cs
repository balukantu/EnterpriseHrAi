using HrAi.Api.Data;
using HrAi.Api.Dtos;
using HrAi.Api.Models;
using Microsoft.EntityFrameworkCore;

namespace HrAi.Api.Repositories;

public class AiPluginExecutionLogRepository : IAiPluginExecutionLogRepository
{
    private readonly HrAiDbContext _context;

    public AiPluginExecutionLogRepository(HrAiDbContext context)
    {
        _context = context;
    }

    public async Task SaveAsync(AiPluginExecutionLog log)
    {
        await _context.AiPluginExecutionLogs.AddAsync(log);
        await _context.SaveChangesAsync();
    }

    public async Task<List<AiPluginUsageSummaryDto>> GetUsageSummaryAsync(
        DateTime fromDate,
        DateTime toDate)
    {
        return await _context.AiPluginExecutionLogs
            .Where(x => x.CreatedAt >= fromDate && x.CreatedAt <= toDate)
            .GroupBy(x => new { x.PluginName, x.FunctionName })
            .Select(g => new AiPluginUsageSummaryDto
            {
                PluginName = g.Key.PluginName,
                FunctionName = g.Key.FunctionName,
                TotalCalls = g.Count(),
                FailedCalls = g.Count(x => x.Status == "Failed"),
                AverageDurationMs = g.Average(x => x.DurationMs)
            })
            .OrderByDescending(x => x.TotalCalls)
            .ToListAsync();
    }
}