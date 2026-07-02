public class AiEvaluationRunResultDto
{
    public int TestCaseId { get; set; }
    public string TestName { get; set; } = string.Empty;
    public string Question { get; set; } = string.Empty;
    public string ExpectedKeyword { get; set; } = string.Empty;
    public string ActualAnswer { get; set; } = string.Empty;
    public bool Passed { get; set; }
    public string? FailureReason { get; set; }
}