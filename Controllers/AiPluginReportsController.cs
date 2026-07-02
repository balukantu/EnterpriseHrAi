using HrAi.Api.Services;
using Microsoft.AspNetCore.Mvc;

namespace HrAi.Api.Controllers;

[ApiController]
[Route("api/ai-plugin-reports")]
public class AiPluginReportsController : ControllerBase
{
    private readonly IAiPluginExecutionLogService _pluginLogService;

    public AiPluginReportsController(
        IAiPluginExecutionLogService pluginLogService)
    {
        _pluginLogService = pluginLogService;
    }

    [HttpGet("usage-summary")]
    public async Task<IActionResult> GetUsageSummary(
        [FromQuery] DateTime fromDate,
        [FromQuery] DateTime toDate)
    {
        var result = await _pluginLogService.GetUsageSummaryAsync(
            fromDate,
            toDate);

        return Ok(result);
    }
}