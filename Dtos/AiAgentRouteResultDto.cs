using HrAi.Api.Models;

namespace HrAi.Api.Dtos;

public class AiAgentRouteResultDto
{
    public AiAgentType AgentType { get; set; }

    public string Reason { get; set; } = string.Empty;
}