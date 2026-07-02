using HrAi.Api.Dtos;
using HrAi.Api.Services;
using Microsoft.AspNetCore.Mvc;

namespace HrAi.Api.Controllers;

[ApiController]
[Route("api/hr-agent")]
public class HrAgentController : ControllerBase
{
    private readonly IHrLeaveAgentService _leaveAgentService;

    public HrAgentController(IHrLeaveAgentService leaveAgentService)
    {
        _leaveAgentService = leaveAgentService;
    }

    [HttpPost("leave-analysis")]
    public async Task<IActionResult> AnalyzeLeave(
        [FromBody] AiChatRequestDto request)
    {
        var result = await _leaveAgentService.AnalyzeLeaveRequestAsync(
            request.EmployeeId,
            request.Message);

        return Ok(result);
    }
}