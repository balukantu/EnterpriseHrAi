namespace HrAi.Api.Services;

public interface IAiAuthorizationService
{
    Task<bool> CanUsePluginAsync(int employeeId, string pluginName);
}