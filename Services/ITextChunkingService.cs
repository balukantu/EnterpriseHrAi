namespace HrAi.Api.Services;

public interface ITextChunkingService
{
    List<string> ChunkText(string text, int maxChunkSize = 800);
}