using HrAi.Api.Dtos;

namespace HrAi.Api.Services;

public interface IPromptVersionService
{
    Task<string> BuildActivePromptAsync(string promptName, int employeeId);

    Task<List<PromptVersionDto>> GetPromptVersionsAsync(string promptName);

    Task<PromptVersionDto> CreateAsync(CreatePromptVersionDto dto);

    Task ActivateAsync(int promptVersionId);
}