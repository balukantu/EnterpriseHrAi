namespace HrAi.Api.Models;

public class PromptVersion
{
    public int PromptVersionId { get; set; }

    public string PromptName { get; set; } = string.Empty;

    public string Version { get; set; } = string.Empty;

    public string Content { get; set; } = string.Empty;

    public bool IsActive { get; set; }

    public string? CreatedBy { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}