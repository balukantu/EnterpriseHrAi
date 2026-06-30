using HrAi.Api.Dtos;

namespace HrAi.Api.Services;

public interface IPolicySearchService
{
    Task<List<PolicySearchResultDto>> SearchPolicyAsync(string question);
}