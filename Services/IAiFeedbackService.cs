using HrAi.Api.Dtos;

namespace HrAi.Api.Services;

public interface IAiFeedbackService
{
    Task SaveAsync(CreateAiFeedbackDto dto);

    Task<AiFeedbackSummaryDto> GetSummaryAsync(DateTime fromDate, DateTime toDate);
}