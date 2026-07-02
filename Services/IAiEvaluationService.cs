public interface IAiEvaluationService
{
    Task<List<AiEvaluationRunResultDto>> RunEvaluationAsync();
}