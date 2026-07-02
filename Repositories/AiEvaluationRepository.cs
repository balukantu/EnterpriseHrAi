using HrAi.Api.Data;
using Microsoft.EntityFrameworkCore;
using System.Linq;

public class AiEvaluationRepository : IAiEvaluationRepository
{
    private readonly HrAiDbContext _context;

    public AiEvaluationRepository(HrAiDbContext context)
    {
        _context = context;
    }

    public async Task<List<AiEvaluationTestCase>> GetActiveTestCasesAsync()
    {
        return await _context.AiEvaluationTestCases
            .Where(x => x.IsActive)
            .ToListAsync();
    }

    public async Task SaveResultAsync(AiEvaluationResult result)
    {
        await _context.AiEvaluationResults.AddAsync(result);
        await _context.SaveChangesAsync();
    }
}