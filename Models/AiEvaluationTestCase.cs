public class AiEvaluationTestCase
{
    public int TestCaseId { get; set; }
    public string TestName { get; set; } = string.Empty;
    public int EmployeeId { get; set; }
    public string Question { get; set; } = string.Empty;
    public string ExpectedKeyword { get; set; } = string.Empty;
    public bool IsActive { get; set; } = true;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}