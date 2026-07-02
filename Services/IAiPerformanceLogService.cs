using HrAi.Api.Dtos;

namespace HrAi.Api.Services;

public interface IAiPerformanceLogService
{
    Task TrackAsync(
        int employeeId,
        Guid? chatSessionId,
        string operationName,
        Func<Task> operation);

    Task<List<AiPerformanceSummaryDto>> GetSummaryAsync(DateTime fromDate, DateTime toDate);
}