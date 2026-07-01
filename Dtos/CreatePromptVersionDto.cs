namespace HrAi.Api.Dtos;

public class CreatePromptVersionDto
{
    public string PromptName { get; set; } = string.Empty;

    public string Version { get; set; } = string.Empty;

    public string Content { get; set; } = string.Empty;

    public string? CreatedBy { get; set; }
}