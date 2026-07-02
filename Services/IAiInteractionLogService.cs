using HrAi.Api.Dtos;
using HrAi.Api.Models;

namespace HrAi.Api.Services;

public interface IAiInteractionLogService
{
    Task SaveAsync(AiInteractionLog log);

    Task<AiCostSummaryDto> GetCostSummaryAsync(DateTime fromDate, DateTime toDate);
}