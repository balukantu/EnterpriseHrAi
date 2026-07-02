using HrAi.Api.Dtos;
using Microsoft.SemanticKernel;
using Microsoft.SemanticKernel.ChatCompletion;
using Microsoft.EntityFrameworkCore;

namespace HrAi.Api.Services;

public class HrLeaveAgentService : IHrLeaveAgentService
{
    private readonly ILeaveService _leaveService;
    private readonly IEmployeeProfileService _employeeProfileService;
    private readonly IVectorPolicySearchService _vectorPolicySearchService;
    private readonly Kernel _kernel;

    public HrLeaveAgentService(
        ILeaveService leaveService,
        IEmployeeProfileService employeeProfileService,
        IVectorPolicySearchService vectorPolicySearchService,
        Kernel kernel)
    {
        _leaveService = leaveService;
        _employeeProfileService = employeeProfileService;
        _vectorPolicySearchService = vectorPolicySearchService;
        _kernel = kernel;
    }

    public async Task<AiAgentResponseDto> AnalyzeLeaveRequestAsync(
        int employeeId,
        string userMessage)
    {
        var response = new AiAgentResponseDto();

        var employeeProfile = await _employeeProfileService
            .GetMyProfileAsync(employeeId);

        var profileText = employeeProfile != null
            ? $"Name: {employeeProfile.FullName}, Department: {employeeProfile.Department}, Manager: {employeeProfile.ManagerName}"
            : "Profile not found";

        response.Steps.Add(new AiAgentStepDto
        {
            StepNumber = 1,
            StepName = "Get Employee Profile",
            Result = profileText
        });

        var leaveBalances = await _leaveService
            .GetLeaveBalancesAsync(employeeId);

        var leaveBalanceText = leaveBalances.Count == 0
            ? "No leave balance found."
            : string.Join(", ", leaveBalances.Select(x => $"{x.LeaveType}: {x.BalanceDays}"));

        response.Steps.Add(new AiAgentStepDto
        {
            StepNumber = 2,
            StepName = "Check Leave Balance",
            Result = leaveBalanceText
        });

        var policyResults = await _vectorPolicySearchService
            .SearchAsync(userMessage);

        var policyText = policyResults.Count == 0
            ? "No matching leave policy found."
            : string.Join("\n\n", policyResults.Select(x =>
                $"""
                Source: {x.DocumentTitle}
                Score: {x.Score:F3}
                Content: {x.Content}
                """));

        response.Steps.Add(new AiAgentStepDto
        {
            StepNumber = 3,
            StepName = "Search Leave Policy",
            Result = policyText
        });

        var chatService = _kernel.GetRequiredService<IChatCompletionService>();

        var leaveBalanceDisplayText = string.Join(", ", leaveBalances.Select(x => $"{x.LeaveType}: {x.BalanceDays}"));

        var prompt = $"""
        You are an enterprise HR leave decision assistant.

        User request:
        {userMessage}

        Employee profile:
        {profileText}

        Leave balance:
        {leaveBalanceDisplayText}

        Relevant leave policy:
        {policyText}

        Task:
        1. Explain whether the employee appears eligible.
        2. Explain what leave balance is available.
        3. Explain policy constraints.
        4. If information is missing, ask a follow-up question.
        5. Do not approve leave directly unless the system has an apply-leave function.
        6. Keep the answer simple and professional.

        Final answer:
        """;

        var finalResult = await chatService.GetChatMessageContentAsync(prompt);

        response.FinalAnswer = finalResult.Content ?? string.Empty;

        response.Steps.Add(new AiAgentStepDto
        {
            StepNumber = 4,
            StepName = "Generate Final Recommendation",
            Result = response.FinalAnswer
        });

        return response;
    }
}