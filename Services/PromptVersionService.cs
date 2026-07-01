using HrAi.Api.Dtos;
using HrAi.Api.Models;
using HrAi.Api.Repositories;

namespace HrAi.Api.Services;

public class PromptVersionService : IPromptVersionService
{
    private readonly IPromptVersionRepository _repository;

    public PromptVersionService(IPromptVersionRepository repository)
    {
        _repository = repository;
    }

    public async Task<string> BuildActivePromptAsync(string promptName, int employeeId)
    {
        var prompt = await _repository.GetActivePromptAsync(promptName);

        if (prompt == null)
            throw new InvalidOperationException($"No active prompt found for {promptName}.");

        return prompt.Content
            .Replace("{{employeeId}}", employeeId.ToString());
    }

    public async Task<List<PromptVersionDto>> GetPromptVersionsAsync(string promptName)
    {
        var prompts = await _repository.GetPromptVersionsAsync(promptName);

        return prompts.Select(x => new PromptVersionDto
        {
            PromptVersionId = x.PromptVersionId,
            PromptName = x.PromptName,
            Version = x.Version,
            Content = x.Content,
            IsActive = x.IsActive,
            CreatedAt = x.CreatedAt
        }).ToList();
    }

    public async Task<PromptVersionDto> CreateAsync(CreatePromptVersionDto dto)
    {
        var prompt = new PromptVersion
        {
            PromptName = dto.PromptName,
            Version = dto.Version,
            Content = dto.Content,
            CreatedBy = dto.CreatedBy,
            IsActive = false,
            CreatedAt = DateTime.UtcNow
        };

        var created = await _repository.CreateAsync(prompt);

        return new PromptVersionDto
        {
            PromptVersionId = created.PromptVersionId,
            PromptName = created.PromptName,
            Version = created.Version,
            Content = created.Content,
            IsActive = created.IsActive,
            CreatedAt = created.CreatedAt
        };
    }

    public async Task ActivateAsync(int promptVersionId)
    {
        await _repository.ActivateAsync(promptVersionId);
    }
}