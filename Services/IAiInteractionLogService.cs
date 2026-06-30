using HrAi.Api.Models;

namespace HrAi.Api.Services;

public interface IAiInteractionLogService
{
    Task SaveAsync(AiInteractionLog log);
}