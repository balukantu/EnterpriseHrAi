using HrAi.Api.Services;
using Microsoft.AspNetCore.Mvc;

namespace HrAi.Api.Controllers;

[ApiController]
[Route("api/ai-reports")]
public class AiReportsController : ControllerBase
{
    private readonly IAiInteractionLogService _aiInteractionLogService;

    public AiReportsController(IAiInteractionLogService aiInteractionLogService)
    {
        _aiInteractionLogService = aiInteractionLogService;
    }

    [HttpGet("cost-summary")]
    public async Task<IActionResult> GetCostSummary(
        [FromQuery] DateTime fromDate,
        [FromQuery] DateTime toDate)
    {
        var result = await _aiInteractionLogService.GetCostSummaryAsync(
            fromDate,
            toDate);

        return Ok(result);
    }
}