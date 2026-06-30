using System.Text.Json;
using HrAi.Api.Data;
using HrAi.Api.Dtos;
using HrAi.Api.Services;
using Microsoft.EntityFrameworkCore;

namespace HrAi.Api.Repositories;

public class VectorPolicySearchRepository : IVectorPolicySearchRepository
{
    private readonly HrAiDbContext _context;

    public VectorPolicySearchRepository(HrAiDbContext context)
    {
        _context = context;
    }

    public async Task<List<VectorSearchResultDto>> SearchAsync(float[] questionEmbedding, int top)
    {
        var chunks = await _context.DocumentChunks
            .Include(x => x.Document)
            .Where(x => x.Embedding != null)
            .ToListAsync();

        var scoredResults = new List<VectorSearchResultDto>();

        foreach (var chunk in chunks)
        {
            var chunkEmbedding = JsonSerializer.Deserialize<float[]>(chunk.Embedding!);

            if (chunkEmbedding == null)
                continue;

            var score = VectorMath.CosineSimilarity(questionEmbedding, chunkEmbedding);

            scoredResults.Add(new VectorSearchResultDto
            {
                DocumentTitle = chunk.Document!.Title,
                PageNumber = chunk.PageNumber,
                Content = chunk.ChunkText,
                Score = score
            });
        }

        return scoredResults
            .OrderByDescending(x => x.Score)
            .Take(top)
            .ToList();
    }

    public async Task<List<int>> GenerateMissingEmbeddingsAsync(
        Func<string, Task<float[]>> embeddingGenerator)
    {
        var chunks = await _context.DocumentChunks
            .Where(x => x.Embedding == null)
            .ToListAsync();

        var updatedIds = new List<int>();

        foreach (var chunk in chunks)
        {
            var embedding = await embeddingGenerator(chunk.ChunkText);
            chunk.Embedding = JsonSerializer.Serialize(embedding);
            updatedIds.Add(chunk.DocumentChunkId);
        }

        await _context.SaveChangesAsync();

        return updatedIds;
    }
}