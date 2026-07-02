using HrAi.Api.Dtos;
using HrAi.Api.Models;

namespace HrAi.Api.Repositories;

public interface IAiPerformanceLogRepository
{
    Task SaveAsync(AiPerformanceLog log);

    Task<List<AiPerformanceSummaryDto>> GetSummaryAsync(DateTime fromDate, DateTime toDate);
}