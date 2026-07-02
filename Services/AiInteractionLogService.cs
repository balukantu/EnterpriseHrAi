using HrAi.Api.Dtos;
using HrAi.Api.Models;
using HrAi.Api.Repositories;

namespace HrAi.Api.Services;

public class AiInteractionLogService : IAiInteractionLogService
{
    private readonly IAiInteractionLogRepository _repository;

    public AiInteractionLogService(IAiInteractionLogRepository repository)
    {
        _repository = repository;
    }

    public async Task SaveAsync(AiInteractionLog log)
    {
        await _repository.SaveAsync(log);
    }

    public async Task<AiCostSummaryDto> GetCostSummaryAsync(
        DateTime fromDate,
        DateTime toDate)
    {
        return await _repository.GetCostSummaryAsync(fromDate, toDate);
    }
}