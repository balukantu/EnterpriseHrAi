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

        var results = await _repository.SearchAsync(
            questionEmbedding,
            top: 3);

        var cacheMinutes = _configuration.GetValue<int>(
            "CacheSettings:RagSearchCacheMinutes",
            30);

        _cacheService.Set(
            cacheKey,
            results,
            TimeSpan.FromMinutes(cacheMinutes));

        return results;
    }

    public async Task<List<int>> GenerateMissingEmbeddingsAsync()
    {
        return await _repository.GenerateMissingEmbeddingsAsync(
            text => _embeddingService.GenerateEmbeddingAsync(text));
    }
}