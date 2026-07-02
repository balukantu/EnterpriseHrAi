using HrAi.Api.Data;
using HrAi.Api.Dtos;
using HrAi.Api.Models;
using Microsoft.EntityFrameworkCore;

namespace HrAi.Api.Repositories;

public class AiFeedbackRepository : IAiFeedbackRepository
{
    private readonly HrAiDbContext _context;

    public AiFeedbackRepository(HrAiDbContext context)
    {
        _context = context;
    }

    public async Task SaveAsync(AiFeedback feedback)
    {
        await _context.AiFeedback.AddAsync(feedback);
        await _context.SaveChangesAsync();
    }

    public async Task<AiFeedbackSummaryDto> GetSummaryAsync(
        DateTime fromDate,
        DateTime toDate)
    {
        var feedback = await _context.AiFeedback
            .Where(x => x.CreatedAt >= fromDate && x.CreatedAt <= toDate)
            .ToListAsync();

        var positive = feedback.Count(x => x.Rating == 1);
        var negative = feedback.Count(x => x.Rating == -1);
        var total = feedback.Count;

        return new AiFeedbackSummaryDto
        {
            TotalFeedback = total,
            PositiveCount = positive,
            NegativeCount = negative,
            PositivePercentage = total == 0
                ? 0
                : Math.Round((double)positive / total * 100, 2)
        };
    }
}