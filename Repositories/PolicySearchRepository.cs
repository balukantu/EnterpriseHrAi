using HrAi.Api.Data;
using HrAi.Api.Dtos;
using Microsoft.EntityFrameworkCore;

namespace HrAi.Api.Repositories;

public class PolicySearchRepository : IPolicySearchRepository
{
    private readonly HrAiDbContext _context;

    public PolicySearchRepository(HrAiDbContext context)
    {
        _context = context;
    }

    public async Task<List<PolicySearchResultDto>> SearchPolicyAsync(string query)
    {
        query = query.Trim().ToLower();

        return await _context.DocumentChunks
            .Include(x => x.Document)
            .Where(x =>
                x.ChunkText.ToLower().Contains(query) ||
                x.Document!.Title.ToLower().Contains(query))
            .OrderBy(x => x.DocumentId)
            .Take(5)
            .Select(x => new PolicySearchResultDto
            {
                DocumentTitle = x.Document!.Title,
                PageNumber = x.PageNumber,
                Content = x.ChunkText
            })
            .ToListAsync();
    }
}