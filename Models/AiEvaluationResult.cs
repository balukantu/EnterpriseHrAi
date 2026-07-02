public class AiEvaluationResult
{
    public Guid EvaluationResultId { get; set; }
    public int TestCaseId { get; set; }
    public string? ActualAnswer { get; set; }
    public bool Passed { get; set; }
    public string? FailureReason { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public AiEvaluationTestCase? TestCase { get; set; }
}