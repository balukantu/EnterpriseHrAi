using HrAi.Api.Dtos;
using HrAi.Api.Repositories;

namespace HrAi.Api.Services;

public class VectorPolicySearchService : IVectorPolicySearchService
{
    private readonly IEmbeddingService _embeddingService;
    private readonly IVectorPolicySearchRepository _repository;

    public VectorPolicySearchService(
        IEmbeddingService embeddingService,
        IVectorPolicySearchRepository repository)
    {
        _embeddingService = embeddingService;
        _repository = repository;
    }

    public async Task<List<VectorSearchResultDto>> SearchAsync(string question)
    {
        var questionEmbedding = await _embeddingService.GenerateEmbeddingAsync(question);

        return await _repository.SearchAsync(questionEmbedding, top: 3);
    }

    public async Task<List<int>> GenerateMissingEmbeddingsAsync()
    {
        return await _repository.GenerateMissingEmbeddingsAsync(
            text => _embeddingService.GenerateEmbeddingAsync(text));
    }
}