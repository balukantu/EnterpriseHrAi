namespace HrAi.Api.Dtos;

public class AiPerformanceSummaryDto
{
    public string OperationName { get; set; } = string.Empty;

    public int TotalCalls { get; set; }

    public double AverageDurationMs { get; set; }

    public int MaxDurationMs { get; set; }

    public int FailedCalls { get; set; }
}