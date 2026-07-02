public interface IAiEvaluationRepository
{
    Task<List<AiEvaluationTestCase>> GetActiveTestCasesAsync();
    Task SaveResultAsync(AiEvaluationResult result);
}