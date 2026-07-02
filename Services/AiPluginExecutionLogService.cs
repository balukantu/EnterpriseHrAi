using HrAi.Api.Dtos;
using HrAi.Api.Models;
using HrAi.Api.Repositories;

namespace HrAi.Api.Services;

public class AiPluginExecutionLogService : IAiPluginExecutionLogService
{
    private readonly IAiPluginExecutionLogRepository _repository;

    public AiPluginExecutionLogService(
        IAiPluginExecutionLogRepository repository)
    {
        _repository = repository;
    }

    public async Task SaveAsync(AiPluginExecutionLog log)
    {
        await _repository.SaveAsync(log);
    }

    public async Task<List<AiPluginUsageSummaryDto>> GetUsageSummaryAsync(
        DateTime fromDate,
        DateTime toDate)
    {
        return await _repository.GetUsageSummaryAsync(fromDate, toDate);
    }
}