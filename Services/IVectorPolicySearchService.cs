using HrAi.Api.Dtos;

namespace HrAi.Api.Services;

public interface IVectorPolicySearchService
{
    Task<List<VectorSearchResultDto>> SearchAsync(string question);
    Task<List<int>> GenerateMissingEmbeddingsAsync();
}