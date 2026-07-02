using HrAi.Api.Dtos;
using HrAi.Api.Services;
using Microsoft.AspNetCore.Mvc;

namespace HrAi.Api.Controllers;

[ApiController]
[Route("api/multi-agent")]
public class MultiAgentController : ControllerBase
{
    private readonly IMultiAgentOrchestratorService _multiAgentService;

    public MultiAgentController(IMultiAgentOrchestratorService multiAgentService)
    {
        _multiAgentService = multiAgentService;
    }

    [HttpPost("chat")]
    public async Task<IActionResult> Chat([FromBody] AiChatRequestDto request)
    {
        var answer = await _multiAgentService.HandleAsync(
            request.EmployeeId,
            request.Message);

        return Ok(new
        {
            answer
        });
    }
}