using HrAi.Api.Data;
using HrAi.Api.Dtos;
using HrAi.Api.Models;
using Microsoft.EntityFrameworkCore;

namespace HrAi.Api.Repositories;

public class AiInteractionLogRepository : IAiInteractionLogRepository
{
    private readonly HrAiDbContext _context;

    public AiInteractionLogRepository(HrAiDbContext context)
    {
        _context = context;
    }

    public async Task SaveAsync(AiInteractionLog log)
    {
        await _context.AiInteractionLogs.AddAsync(log);
        await _context.SaveChangesAsync();
    }

    public async Task<AiCostSummaryDto> GetCostSummaryAsync(
        DateTime fromDate,
        DateTime toDate)
    {
        var logs = await _context.AiInteractionLogs
            .Where(x => x.CreatedAt >= fromDate && x.CreatedAt <= toDate)
            .ToListAsync();

        return new AiCostSummaryDto
        {
            TotalRequests = logs.Count,
            TotalPromptTokens = logs.Sum(x => x.PromptTokens ?? 0),
            TotalCompletionTokens = logs.Sum(x => x.CompletionTokens ?? 0),
            TotalTokens = logs.Sum(x => x.TotalTokens ?? 0),
            TotalEstimatedCost = logs.Sum(x => x.EstimatedCost ?? 0)
        };
    }
}