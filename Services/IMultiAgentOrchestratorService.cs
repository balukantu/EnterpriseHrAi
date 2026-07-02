namespace HrAi.Api.Services;

public interface IMultiAgentOrchestratorService
{
    Task<string> HandleAsync(int employeeId, string message);
}