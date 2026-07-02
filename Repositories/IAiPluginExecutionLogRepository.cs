using HrAi.Api.Dtos;
using HrAi.Api.Models;

namespace HrAi.Api.Repositories;

public interface IAiPluginExecutionLogRepository
{
    Task SaveAsync(AiPluginExecutionLog log);

    Task<List<AiPluginUsageSummaryDto>> GetUsageSummaryAsync(
        DateTime fromDate,
        DateTime toDate);
}