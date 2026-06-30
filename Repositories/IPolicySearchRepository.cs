using HrAi.Api.Dtos;

namespace HrAi.Api.Repositories;

public interface IPolicySearchRepository
{
    Task<List<PolicySearchResultDto>> SearchPolicyAsync(string query);
}