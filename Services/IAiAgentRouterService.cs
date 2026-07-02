using HrAi.Api.Dtos;

namespace HrAi.Api.Services;

public interface IAiAgentRouterService
{
    AiAgentRouteResultDto Route(string userMessage);
}