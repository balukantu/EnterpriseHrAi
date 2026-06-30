using System.ComponentModel;
using HrAi.Api.Services;
using Microsoft.SemanticKernel;

namespace HrAi.Api.Plugins;

public class EmployeePlugin
{
    private readonly IEmployeeProfileService _employeeProfileService;

    public EmployeePlugin(IEmployeeProfileService employeeProfileService)
    {
        _employeeProfileService = employeeProfileService;
    }

    [KernelFunction]
    [Description("Use this function when the logged-in employee asks for their own employee profile, department, email, hire date, or basic employee details.")]
    public async Task<string> GetMyProfileAsync(
        [Description("Logged-in employee ID.")] int employeeId)
    {
        var profile = await _employeeProfileService.GetMyProfileAsync(employeeId);

        if (profile == null)
            return "Employee profile not found.";

        return $"""
        Employee Profile:
        Name: {profile.FullName}
        Email: {profile.Email}
        Department: {profile.Department}
        Manager: {profile.ManagerName ?? "Not assigned"}
        Manager Email: {profile.ManagerEmail ?? "Not available"}
        Hire Date: {profile.HireDate:yyyy-MM-dd}
        """;
    }

    [KernelFunction]
    [Description("Use this function when the logged-in employee asks who their manager is or asks for manager details.")]
    public async Task<string> GetMyManagerAsync(
        [Description("Logged-in employee ID.")] int employeeId)
    {
        return await _employeeProfileService.GetMyManagerAsync(employeeId);
    }
}