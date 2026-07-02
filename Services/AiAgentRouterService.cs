using HrAi.Api.Dtos;
using HrAi.Api.Models;

namespace HrAi.Api.Services;

public class AiAgentRouterService : IAiAgentRouterService
{
    public AiAgentRouteResultDto Route(string userMessage)
    {
        var message = userMessage.ToLowerInvariant();

        if (message.Contains("leave") ||
            message.Contains("vacation") ||
            message.Contains("sick") ||
            message.Contains("pto"))
        {
            return new AiAgentRouteResultDto
            {
                AgentType = AiAgentType.Leave,
                Reason = "Leave-related question detected."
            };
        }

        if (message.Contains("salary") ||
            message.Contains("payroll") ||
            message.Contains("tax") ||
            message.Contains("payslip"))
        {
            return new AiAgentRouteResultDto
            {
                AgentType = AiAgentType.Payroll,
                Reason = "Payroll-related question detected."
            };
        }

        if (message.Contains("policy") ||
            message.Contains("work from home") ||
            message.Contains("remote work") ||
            message.Contains("benefits") ||
            message.Contains("handbook"))
        {
            return new AiAgentRouteResultDto
            {
                AgentType = AiAgentType.Policy,
                Reason = "Policy-related question detected."
            };
        }

        if (message.Contains("manager") ||
            message.Contains("department") ||
            message.Contains("profile") ||
            message.Contains("hire date"))
        {
            return new AiAgentRouteResultDto
            {
                AgentType = AiAgentType.EmployeeProfile,
                Reason = "Employee-profile question detected."
            };
        }

        return new AiAgentRouteResultDto
        {
            AgentType = AiAgentType.General,
            Reason = "General HR question."
        };
    }
}