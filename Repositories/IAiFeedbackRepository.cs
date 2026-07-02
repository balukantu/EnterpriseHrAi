using HrAi.Api.Dtos;
using HrAi.Api.Models;

namespace HrAi.Api.Repositories;

public interface IAiFeedbackRepository
{
    Task SaveAsync(AiFeedback feedback);

    Task<AiFeedbackSummaryDto> GetSummaryAsync(DateTime fromDate, DateTime toDate);
}