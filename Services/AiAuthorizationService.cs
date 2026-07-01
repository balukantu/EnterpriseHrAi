using HrAi.Api.Repositories;

namespace HrAi.Api.Services;

public class AiAuthorizationService : IAiAuthorizationService
{
    private readonly IEmployeeRepository _employeeRepository;

    public AiAuthorizationService(IEmployeeRepository employeeRepository)
    {
        _employeeRepository = employeeRepository;
    }

    public async Task<bool> CanUsePluginAsync(int employeeId, string pluginName)
    {
        var employee = await _employeeRepository.GetByIdAsync(employeeId);

        if (employee == null)
            return false;

        if (employee.RoleName == "HRAdmin")
            return true;

        return pluginName switch
        {
            "LeavePlugin" => true,
            "EmployeePlugin" => true,
            "PayrollPlugin" => true,
            "PolicySearchPlugin" => true,
            "AdminPlugin" => false,
            _ => false
        };
    }
}