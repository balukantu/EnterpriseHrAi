namespace HrAi.Api.Dtos;

public class AiChatResponseDto
{
    public Guid ChatSessionId { get; set; }

    public Guid LogId { get; set; }

    public string Answer { get; set; } = string.Empty;

    public int? ResponseTimeMs { get; set; }

    public string? ModelName { get; set; }
}