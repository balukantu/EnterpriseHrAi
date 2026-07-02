namespace HrAi.Api.Services;

public class LeaveAgent : IAiAgent
{
    private readonly ILeaveService _leaveService;
    private readonly IVectorPolicySearchService _policySearchService;

    public LeaveAgent(
        ILeaveService leaveService,
        IVectorPolicySearchService policySearchService)
    {
        _leaveService = leaveService;
        _policySearchService = policySearchService;
    }

    public async Task<string> HandleAsync(int employeeId, string message)
    {
        var leaveBalance = await _leaveService.GetLeaveBalancesAsync(employeeId);
        var policies = await _policySearchService.SearchAsync(message);

        var policyText = policies.Count == 0
            ? "No matching leave policy found."
            : string.Join("\n\n", policies.Select(x => x.Content));

        return $"""
        Leave Agent Result:

        Leave Balance:
        {leaveBalance}

        Related Policy:
        {policyText}
        """;
    }
}