using System.ComponentModel;
using HrAi.Api.Services;
using Microsoft.SemanticKernel;

namespace HrAi.Api.Plugins;

public class PolicySearchPlugin
{
    private readonly IVectorPolicySearchService _vectorPolicySearchService;

    public PolicySearchPlugin(IVectorPolicySearchService vectorPolicySearchService)
    {
        _vectorPolicySearchService = vectorPolicySearchService;
    }

    [KernelFunction]
    [Description("Use this function when the employee asks about company policies, HR policies, leave policy, parental leave, sick leave, remote work, work from home, benefits, handbook, or policy documents.")]
    public async Task<string> SearchPolicyAsync(
    [Description("The employee policy question.")] string question)
    {
        var results = await _vectorPolicySearchService.SearchAsync(question);

        if (results.Count == 0)
        {
            return """
        No matching policy information found with enough confidence.
        Do not guess. Tell the user that no matching policy information was found.
        """;
        }

        return string.Join("\n\n", results.Select(x =>
            $"""
            Source: {x.DocumentTitle}, Page: {x.PageNumber}, Similarity Score: {x.Score:F3}
            Content: {x.Content}
            """));
    }
}