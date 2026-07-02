namespace HrAi.Api.Dtos;

public class AiFeedbackSummaryDto
{
    public int TotalFeedback { get; set; }

    public int PositiveCount { get; set; }

    public int NegativeCount { get; set; }

    public double PositivePercentage { get; set; }
}