using HrAi.Api.Dtos;
using HrAi.Api.Services;

public class AiEvaluationService : IAiEvaluationService
{
    private readonly IAiEvaluationRepository _repository;
    private readonly IAiOrchestratorService _aiOrchestratorService;

    public AiEvaluationService(
        IAiEvaluationRepository repository,
        IAiOrchestratorService aiOrchestratorService)
    {
        _repository = repository;
        _aiOrchestratorService = aiOrchestratorService;
    }

    public async Task<List<AiEvaluationRunResultDto>> RunEvaluationAsync()
    {
        var testCases = await _repository.GetActiveTestCasesAsync();
        var results = new List<AiEvaluationRunResultDto>();

        foreach (var testCase in testCases)
        {
            var response = await _aiOrchestratorService.ChatAsync(new AiChatRequestDto
            {
                EmployeeId = testCase.EmployeeId,
                ChatSessionId = null,
                Message = testCase.Question
            });

            var passed = response.Answer.Contains(
                testCase.ExpectedKeyword,
                StringComparison.OrdinalIgnoreCase);

            var failureReason = passed
                ? null
                : $"Expected keyword '{testCase.ExpectedKeyword}' was not found.";

            await _repository.SaveResultAsync(new AiEvaluationResult
            {
                EvaluationResultId = Guid.NewGuid(),
                TestCaseId = testCase.TestCaseId,
                ActualAnswer = response.Answer,
                Passed = passed,
                FailureReason = failureReason,
                CreatedAt = DateTime.UtcNow
            });

            results.Add(new AiEvaluationRunResultDto
            {
                TestCaseId = testCase.TestCaseId,
                TestName = testCase.TestName,
                Question = testCase.Question,
                ExpectedKeyword = testCase.ExpectedKeyword,
                ActualAnswer = response.Answer,
                Passed = passed,
                FailureReason = failureReason
            });
        }

        return results;
    }
}