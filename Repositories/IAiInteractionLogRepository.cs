using HrAi.Api.Models;

namespace HrAi.Api.Repositories;

public interface IAiInteractionLogRepository
{
    Task SaveAsync(AiInteractionLog log);
}