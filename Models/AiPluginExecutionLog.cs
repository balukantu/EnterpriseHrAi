namespace HrAi.Api.Models;

public class AiPluginExecutionLog
{
    public Guid PluginExecutionLogId { get; set; }

    public Guid? LogId { get; set; }

    public int EmployeeId { get; set; }

    public Guid? ChatSessionId { get; set; }

    public string PluginName { get; set; } = string.Empty;

    public string FunctionName { get; set; } = string.Empty;

    public string? InputJson { get; set; }

    public string? OutputText { get; set; }

    public int DurationMs { get; set; }

    public string Status { get; set; } = "Success";

    public string? ErrorMessage { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public AiInteractionLog? AiInteractionLog { get; set; }
}