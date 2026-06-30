using HrAi.Api.Services;
using Microsoft.AspNetCore.Mvc;

namespace HrAi.Api.Controllers;

[ApiController]
[Route("api/employees")]
public class EmployeesController : ControllerBase
{
    private readonly IEmployeeService _employeeService;

    public EmployeesController(IEmployeeService employeeService)
    {
        _employeeService = employeeService;
    }

    [HttpGet]
    public async Task<IActionResult> GetEmployees()
    {
        var employees = await _employeeService.GetEmployeesAsync();
        return Ok(employees);
    }

    [HttpGet("{employeeId:int}")]
    public async Task<IActionResult> GetEmployee(int employeeId)
    {
        var employee = await _employeeService.GetEmployeeAsync(employeeId);

        if (employee == null)
            return NotFound("Employee not found.");

        return Ok(employee);
    }
}