using HrAi.Api.Dtos;

namespace HrAi.Api.Services;

public interface IAiOrchestratorService
{
    Task<AiChatResponseDto> ChatAsync(AiChatRequestDto request);

    IAsyncEnumerable<string> ChatStreamAsync(
        AiChatRequestDto request,
        CancellationToken cancellationToken);
}