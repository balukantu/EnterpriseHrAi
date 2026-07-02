namespace HrAi.Api.Services;

public interface ILlmRoutingPolicyService
{
    string GetServiceId(string userMessage);
}