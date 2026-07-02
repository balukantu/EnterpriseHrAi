namespace HrAi.Api.Models;

public class AiFeedback
{
    public Guid FeedbackId { get; set; }

    public Guid LogId { get; set; }

    public int EmployeeId { get; set; }

    public int Rating { get; set; }

    public string? Comment { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public AiInteractionLog? AiInteractionLog { get; set; }
}