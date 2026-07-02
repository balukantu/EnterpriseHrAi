namespace HrAi.Api.Dtos;

public class AiCostSummaryDto
{
    public int TotalRequests { get; set; }

    public int TotalPromptTokens { get; set; }

    public int TotalCompletionTokens { get; set; }

    public int TotalTokens { get; set; }

    public decimal TotalEstimatedCost { get; set; }
}