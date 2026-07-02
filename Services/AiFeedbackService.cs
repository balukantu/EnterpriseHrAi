using HrAi.Api.Dtos;
using HrAi.Api.Models;
using HrAi.Api.Repositories;

namespace HrAi.Api.Services;

public class AiFeedbackService : IAiFeedbackService
{
    private readonly IAiFeedbackRepository _repository;

    public AiFeedbackService(IAiFeedbackRepository repository)
    {
        _repository = repository;
    }

    public async Task SaveAsync(CreateAiFeedbackDto dto)
    {
        if (dto.Rating != 1 && dto.Rating != -1)
            throw new ArgumentException("Rating must be 1 or -1.");

        var feedback = new AiFeedback
        {
            FeedbackId = Guid.NewGuid(),
            LogId = dto.LogId,
            EmployeeId = dto.EmployeeId,
            Rating = dto.Rating,
            Comment = dto.Comment,
            CreatedAt = DateTime.UtcNow
        };

        await _repository.SaveAsync(feedback);
    }

    public async Task<AiFeedbackSummaryDto> GetSummaryAsync(
        DateTime fromDate,
        DateTime toDate)
    {
        return await _repository.GetSummaryAsync(fromDate, toDate);
    }
}