using HrAi.Api.Dtos;

namespace HrAi.Api.Services;

public interface IHrLeaveAgentService
{
    Task<AiAgentResponseDto> AnalyzeLeaveRequestAsync(
        int employeeId,
        string userMessage);
}