using HrAi.Api.Dtos;
using HrAi.Api.Repositories;

namespace HrAi.Api.Services;

public class PolicySearchService : IPolicySearchService
{
    private readonly IPolicySearchRepository _policySearchRepository;

    public PolicySearchService(IPolicySearchRepository policySearchRepository)
    {
        _policySearchRepository = policySearchRepository;
    }

    public async Task<List<PolicySearchResultDto>> SearchPolicyAsync(string question)
    {
        return await _policySearchRepository.SearchPolicyAsync(question);
    }
}