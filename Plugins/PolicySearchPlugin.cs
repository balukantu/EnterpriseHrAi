using System.ComponentModel;
using HrAi.Api.Services;
using Microsoft.SemanticKernel;

namespace HrAi.Api.Plugins;

public class PolicySearchPlugin
{
    private readonly IPolicySearchService _policySearchService;
    private readonly IVectorPolicySearchService _vectorPolicySearchService;

    public PolicySearchPlugin(IPolicySearchService policySearchService, IVectorPolicySearchService vectorPolicySearchService)
    {
        _policySearchService = policySearchService;
        _vectorPolicySearchService = vectorPolicySearchService;
    }

    //[KernelFunction]
    //[Description("Use this function when the employee asks questions about company policies, HR policies, leave policy, parental leave, sick leave, remote work policy, benefits, handbook, or policy documents.")]
    //public async Task<string> SearchPolicyAsync(
    //    [Description("The employee policy question or search terms.")] string question)
    //{
    //    var results = await _policySearchService.SearchPolicyAsync(question);

    //    if (results.Count == 0)
    //        return "No matching policy information found.";

    //    return string.Join("\n\n", results.Select(x =>
    //        $"""
    //        Source: {x.DocumentTitle}, Page: {x.PageNumber}
    //        Content: {x.Content}
    //        """));
    //}

    [KernelFunction]
    [Description("Use this function when the employee asks about company policies, HR policies, leave policy, parental leave, sick leave, remote work, work from home, benefits, handbook, or policy documents.")]
    public async Task<string> SearchPolicyAsync(
    [Description("The employee policy question.")] string question)
    {
        var results = await _vectorPolicySearchService.SearchAsync(question);

        if (results.Count == 0)
            return "No matching policy information found.";

        return string.Join("\n\n", results.Select(x =>
            $"""
            Source: {x.DocumentTitle}, Page: {x.PageNumber}, Similarity Score: {x.Score:F3}
            Content: {x.Content}
            """));
    }
}