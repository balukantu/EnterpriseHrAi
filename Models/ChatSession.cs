namespace HrAi.Api.Models;

public class ChatSession
{
    public Guid ChatSessionId { get; set; }

    public int EmployeeId { get; set; }

    public string? Title { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

    public Employee? Employee { get; set; }

    public ICollection<ChatMessage> Messages { get; set; } = new List<ChatMessage>();
}