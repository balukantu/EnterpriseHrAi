namespace HrAi.Api.Models;

public class AiPerformanceLog
{
    public Guid AiPerformanceLogId { get; set; }

    public int EmployeeId { get; set; }

    public Guid? ChatSessionId { get; set; }

    public string OperationName { get; set; } = string.Empty;

    public int DurationMs { get; set; }

    public string Status { get; set; } = "Success";

    public string? ErrorMessage { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}