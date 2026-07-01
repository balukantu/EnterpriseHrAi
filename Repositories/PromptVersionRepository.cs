using HrAi.Api.Data;
using HrAi.Api.Models;
using Microsoft.EntityFrameworkCore;

namespace HrAi.Api.Repositories;

public class PromptVersionRepository : IPromptVersionRepository
{
    private readonly HrAiDbContext _context;

    public PromptVersionRepository(HrAiDbContext context)
    {
        _context = context;
    }

    public async Task<PromptVersion?> GetActivePromptAsync(string promptName)
    {
        return await _context.PromptVersions
            .FirstOrDefaultAsync(x => x.PromptName == promptName && x.IsActive);
    }

    public async Task<List<PromptVersion>> GetPromptVersionsAsync(string promptName)
    {
        return await _context.PromptVersions
            .Where(x => x.PromptName == promptName)
            .OrderByDescending(x => x.CreatedAt)
            .ToListAsync();
    }

    public async Task<PromptVersion> CreateAsync(PromptVersion promptVersion)
    {
        promptVersion.IsActive = false;
        promptVersion.CreatedAt = DateTime.UtcNow;

        await _context.PromptVersions.AddAsync(promptVersion);
        await _context.SaveChangesAsync();

        return promptVersion;
    }

    public async Task ActivateAsync(int promptVersionId)
    {
        var prompt = await _context.PromptVersions
            .FirstOrDefaultAsync(x => x.PromptVersionId == promptVersionId);

        if (prompt == null)
            throw new InvalidOperationException("Prompt version not found.");

        var activePrompts = await _context.PromptVersions
            .Where(x => x.PromptName == prompt.PromptName && x.IsActive)
            .ToListAsync();

        foreach (var activePrompt in activePrompts)
        {
            activePrompt.IsActive = false;
        }

        prompt.IsActive = true;

        await _context.SaveChangesAsync();
    }
}