namespace HrAi.Api.Dtos;

public class AiPluginUsageSummaryDto
{
    public string PluginName { get; set; } = string.Empty;

    public string FunctionName { get; set; } = string.Empty;

    public int TotalCalls { get; set; }

    public int FailedCalls { get; set; }

    public double AverageDurationMs { get; set; }
}