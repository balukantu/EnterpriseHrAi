public class AiInteractionLog
{
    public Guid LogId { get; set; }

    public int EmployeeId { get; set; }

    public Guid? ChatSessionId { get; set; }

    public string UserQuestion { get; set; } = string.Empty;

    public string? PluginsUsed { get; set; }

    public string? RetrievedDocuments { get; set; }

    public string? SimilarityScores { get; set; }

    public string? Prompt { get; set; }

    public string? AiResponse { get; set; }

    public string? ModelName { get; set; }

    public int? PromptTokens { get; set; }

    public int? CompletionTokens { get; set; }

    public int? TotalTokens { get; set; }

    public int? ResponseTimeMs { get; set; }

    public string Status { get; set; } = "Success";

    public string? ErrorMessage { get; set; }

    public DateTime CreatedAt { get; set; }
}