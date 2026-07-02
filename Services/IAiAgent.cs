namespace HrAi.Api.Services;

public interface IAiAgent
{
    Task<string> HandleAsync(int employeeId, string message);
}