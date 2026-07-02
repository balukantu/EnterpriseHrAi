using HrAi.Api.Dtos;
using HrAi.Api.Repositories;

namespace HrAi.Api.Services;

public class VectorPolicySearchService : IVectorPolicySearchService
{
    private readonly IEmbeddingService _embeddingService;
    private readonly IVectorPolicySearchRepository _repository;
    private readonly ICacheService _cacheService;
    private readonly IConfiguration _configuration;

    public VectorPolicySearchService(
        IEmbeddingService embeddingService,
        IVectorPolicySearchRepository repository,
        ICacheService cacheService,
        IConfiguration configuration)
    {
        _embeddingService = embeddingService;
        _repository = repository;
        _cacheService = cacheService;
        _configuration = configuration;
    }

    public async Task<List<VectorSearchResultDto>> SearchAsync(string question)
    {
        var normalizedQuestion = question.Trim().ToLowerInvariant();
        var cacheKey = $"rag-search:{normalizedQuestion}";

        if (_cacheService.TryGet(cacheKey, out List<VectorSearchResultDto>? cachedResults))
        {
            return cachedResults!;
        }

        var questionEmbedding = await _embeddingService.GenerateEmbeddingAsync(question);

        var topK = _configuration.GetValue<int>("RagSettings:TopK", 5);
        var minimumScore = _configuration.GetValue<double>(
            "RagSettings:MinimumSimilarityScore",
            0.75);

        var results = await _repository.SearchAsync(
            questionEmbedding,
            topK);

        var filteredResults = results
            .Where(x => x.Score >= minimumScore)
            .OrderByDescending(x => x.Score)
            .ToList();

        var cacheMinutes = _configuration.GetValue<int>(
            "CacheSettings:RagSearchCacheMinutes",
            30);

        _cacheService.Set(
            cacheKey,
            filteredResults,
            TimeSpan.FromMinutes(cacheMinutes));

        return filteredResults;
    }

    public async Task<List<int>> GenerateMissingEmbeddingsAsync()
    {
        return await _repository.GenerateMissingEmbeddingsAsync(
            text => _embeddingService.GenerateEmbeddingAsync(text));
    }
}