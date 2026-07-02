using HrAi.Api.Dtos;
using HrAi.Api.Models;

namespace HrAi.Api.Repositories;

public interface IAiInteractionLogRepository
{
    Task SaveAsync(AiInteractionLog log);

    Task<AiCostSummaryDto> GetCostSummaryAsync(DateTime fromDate, DateTime toDate);
}