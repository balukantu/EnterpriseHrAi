namespace HrAi.Api.Dtos;

public class AiChatResponseDto
{
    public Guid ChatSessionId { get; set; }

    public string Answer { get; set; } = string.Empty;
}