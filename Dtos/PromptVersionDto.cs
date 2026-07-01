namespace HrAi.Api.Dtos;

public class PromptVersionDto
{
    public int PromptVersionId { get; set; }

    public string PromptName { get; set; } = string.Empty;

    public string Version { get; set; } = string.Empty;

    public string Content { get; set; } = string.Empty;

    public bool IsActive { get; set; }

    public DateTime CreatedAt { get; set; }
}