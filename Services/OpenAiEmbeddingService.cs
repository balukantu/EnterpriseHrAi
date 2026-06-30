using OpenAI.Embeddings;

namespace HrAi.Api.Services;

public class OpenAiEmbeddingService : IEmbeddingService
{
    private readonly EmbeddingClient _embeddingClient;

    public OpenAiEmbeddingService(IConfiguration configuration)
    {
        var apiKey = configuration["OpenAI:ApiKey"]
            ?? throw new InvalidOperationException("OpenAI API key is missing.");

        var modelId = configuration["OpenAI:EmbeddingModelId"]
            ?? throw new InvalidOperationException("OpenAI embedding model is missing.");

        _embeddingClient = new EmbeddingClient(modelId, apiKey);
    }

    public async Task<float[]> GenerateEmbeddingAsync(string text)
    {
        var result = await _embeddingClient.GenerateEmbeddingAsync(text);

        return result.Value.ToFloats().ToArray();
    }
}