namespace HrAi.Api.Services;

public class PolicyAgent : IAiAgent
{
    private readonly IVectorPolicySearchService _policySearchService;

    public PolicyAgent(IVectorPolicySearchService policySearchService)
    {
        _policySearchService = policySearchService;
    }

    public async Task<string> HandleAsync(int employeeId, string message)
    {
        var results = await _policySearchService.SearchAsync(message);

        if (results.Count == 0)
            return "No matching policy information found.";

        return string.Join("\n\n", results.Select(x =>
            $"""
            Source: {x.DocumentTitle}
            Score: {x.Score:F3}
            Content: {x.Content}
            """));
    }
}