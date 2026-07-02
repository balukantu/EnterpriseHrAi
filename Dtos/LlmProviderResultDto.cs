namespace HrAi.Api.Dtos;

public class LlmProviderResultDto
{
    public string Answer { get; set; } = string.Empty;

    public string ServiceId { get; set; } = string.Empty;

    public bool UsedFallback { get; set; }

    public string? ErrorMessage { get; set; }
}