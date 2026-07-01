using HrAi.Api.Models;

namespace HrAi.Api.Repositories;

public interface IPromptVersionRepository
{
    Task<PromptVersion?> GetActivePromptAsync(string promptName);

    Task<List<PromptVersion>> GetPromptVersionsAsync(string promptName);

    Task<PromptVersion> CreateAsync(PromptVersion promptVersion);

    Task ActivateAsync(int promptVersionId);
}