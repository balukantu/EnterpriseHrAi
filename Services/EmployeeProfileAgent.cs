namespace HrAi.Api.Services;

public class EmployeeProfileAgent : IAiAgent
{
    private readonly IEmployeeProfileService _employeeProfileService;

    public EmployeeProfileAgent(IEmployeeProfileService employeeProfileService)
    {
        _employeeProfileService = employeeProfileService;
    }

    public async Task<string> HandleAsync(int employeeId, string message)
    {
        var profile = await _employeeProfileService.GetMyProfileAsync(employeeId);

        return $"""
        Employee Profile Agent Result:

        {profile}
        """;
    }
}