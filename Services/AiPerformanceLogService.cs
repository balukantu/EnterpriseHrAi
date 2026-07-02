using System.Diagnostics;
using HrAi.Api.Dtos;
using HrAi.Api.Models;
using HrAi.Api.Repositories;

namespace HrAi.Api.Services;

public class AiPerformanceLogService : IAiPerformanceLogService
{
    private readonly IAiPerformanceLogRepository _repository;

    public AiPerformanceLogService(IAiPerformanceLogRepository repository)
    {
        _repository = repository;
    }

    public async Task TrackAsync(
        int employeeId,
        Guid? chatSessionId,
        string operationName,
        Func<Task> operation)
    {
        var stopwatch = Stopwatch.StartNew();

        try
        {
            await operation();

            stopwatch.Stop();

            await _repository.SaveAsync(new AiPerformanceLog
            {
                AiPerformanceLogId = Guid.NewGuid(),
                EmployeeId = employeeId,
                ChatSessionId = chatSessionId,
                OperationName = operationName,
                DurationMs = (int)stopwatch.ElapsedMilliseconds,
                Status = "Success",
                CreatedAt = DateTime.UtcNow
            });
        }
        catch (Exception ex)
        {
            stopwatch.Stop();

            await _repository.SaveAsync(new AiPerformanceLog
            {
                AiPerformanceLogId = Guid.NewGuid(),
                EmployeeId = employeeId,
                ChatSessionId = chatSessionId,
                OperationName = operationName,
                DurationMs = (int)stopwatch.ElapsedMilliseconds,
                Status = "Failed",
                ErrorMessage = ex.Message,
                CreatedAt = DateTime.UtcNow
            });

            throw;
        }
    }

    public async Task<List<AiPerformanceSummaryDto>> GetSummaryAsync(
        DateTime fromDate,
        DateTime toDate)
    {
        return await _repository.GetSummaryAsync(fromDate, toDate);
    }
}