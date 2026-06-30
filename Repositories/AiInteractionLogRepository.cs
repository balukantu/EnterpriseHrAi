using HrAi.Api.Data;
using HrAi.Api.Models;

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
}