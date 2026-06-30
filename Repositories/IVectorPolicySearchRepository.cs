using HrAi.Api.Dtos;

namespace HrAi.Api.Repositories;

public interface IVectorPolicySearchRepository
{
    Task<List<VectorSearchResultDto>> SearchAsync(float[] questionEmbedding, int top);
    Task<List<int>> GenerateMissingEmbeddingsAsync(Func<string, Task<float[]>> embeddingGenerator);
}