using HrAi.Api.Dtos;
using HrAi.Api.Services;
using Microsoft.AspNetCore.Mvc;

namespace HrAi.Api.Controllers;

[ApiController]
[Route("api/leave")]
public class LeaveController : ControllerBase
{
    private readonly ILeaveService _leaveService;

    public LeaveController(ILeaveService leaveService)
    {
        _leaveService = leaveService;
    }

    [HttpGet("balance/{employeeId:int}")]
    public async Task<IActionResult> GetLeaveBalances(int employeeId)
    {
        var balances = await _leaveService.GetLeaveBalancesAsync(employeeId);
        return Ok(balances);
    }

    [HttpPost("apply")]
    public async Task<IActionResult> ApplyLeave([FromBody] CreateLeaveRequestDto request)
    {
        var result = await _leaveService.ApplyLeaveAsync(request);
        return Ok(new { message = result });
    }
}