using HrAi.Api.Models;
using Microsoft.SemanticKernel;
using Microsoft.SemanticKernel.ChatCompletion;

namespace HrAi.Api.Services;

public class MultiAgentOrchestratorService : IMultiAgentOrchestratorService
{
    private readonly IAiAgentRouterService _router;
    private readonly LeaveAgent _leaveAgent;
    private readonly PayrollAgent _payrollAgent;
    private readonly PolicyAgent _policyAgent;
    private readonly EmployeeProfileAgent _employeeProfileAgent;
    private readonly Kernel _kernel;

    public MultiAgentOrchestratorService(
        IAiAgentRouterService router,
        LeaveAgent leaveAgent,
        PayrollAgent payrollAgent,
        PolicyAgent policyAgent,
        EmployeeProfileAgent employeeProfileAgent,
        Kernel kernel)
    {
        _router = router;
        _leaveAgent = leaveAgent;
        _payrollAgent = payrollAgent;
        _policyAgent = policyAgent;
        _employeeProfileAgent = employeeProfileAgent;
        _kernel = kernel;
    }

    public async Task<string> HandleAsync(int employeeId, string message)
    {
        var route = _router.Route(message);

        string agentResult = route.AgentType switch
        {
            AiAgentType.Leave => await _leaveAgent.HandleAsync(employeeId, message),
            AiAgentType.Payroll => await _payrollAgent.HandleAsync(employeeId, message),
            AiAgentType.Policy => await _policyAgent.HandleAsync(employeeId, message),
            AiAgentType.EmployeeProfile => await _employeeProfileAgent.HandleAsync(employeeId, message),
            _ => "No specialized agent was selected."
        };

        var chatService = _kernel.GetRequiredService<IChatCompletionService>();

        var prompt = $"""
        You are an enterprise HR Copilot.

        User question:
        {message}

        Agent selected:
        {route.AgentType}

        Routing reason:
        {route.Reason}

        Agent result:
        {agentResult}

        Write a clear, professional final answer for the employee.
        Do not invent information beyond the agent result.
        """;

        var response = await chatService.GetChatMessageContentAsync(prompt);

        return response.Content ?? string.Empty;
    }
}