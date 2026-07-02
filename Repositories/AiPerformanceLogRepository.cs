using HrAi.Api.Data;
using HrAi.Api.Dtos;
using HrAi.Api.Models;
using Microsoft.EntityFrameworkCore;

namespace HrAi.Api.Repositories;

public class AiPerformanceLogRepository : IAiPerformanceLogRepository
{
    private readonly HrAiDbContext _context;

    public AiPerformanceLogRepository(HrAiDbContext context)
    {
        _context = context;
    }

    public async Task SaveAsync(AiPerformanceLog log)
    {
        await _context.AiPerformanceLogs.AddAsync(log);
        await _context.SaveChangesAsync();
    }

    public async Task<List<AiPerformanceSummaryDto>> GetSummaryAsync(
        DateTime fromDate,
        DateTime toDate)
    {
        return await _context.AiPerformanceLogs
            .Where(x => x.CreatedAt >= fromDate && x.CreatedAt <= toDate)
            .GroupBy(x => x.OperationName)
            .Select(g => new AiPerformanceSummaryDto
            {
                OperationName = g.Key,
                TotalCalls = g.Count(),
                AverageDurationMs = g.Average(x => x.DurationMs),
                MaxDurationMs = g.Max(x => x.DurationMs),
                FailedCalls = g.Count(x => x.Status == "Failed")
            })
            .OrderByDescending(x => x.AverageDurationMs)
            .ToListAsync();
    }
}