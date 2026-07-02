using HrAi.Api.Dtos;
using HrAi.Api.Models;

namespace HrAi.Api.Services;

public interface IAiPluginExecutionLogService
{
    Task SaveAsync(AiPluginExecutionLog log);

    Task<List<AiPluginUsageSummaryDto>> GetUsageSummaryAsync(
        DateTime fromDate,
        DateTime toDate);
}