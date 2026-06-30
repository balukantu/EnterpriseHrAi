namespace HrAi.Api.Models;

public class ChatMessage
{
    public long ChatMessageId { get; set; }

    public Guid ChatSessionId { get; set; }

    public string Role { get; set; } = string.Empty;

    public string Content { get; set; } = string.Empty;

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public ChatSession? ChatSession { get; set; }
}