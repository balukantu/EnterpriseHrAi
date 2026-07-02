using HrAi.Api.Services;
using Microsoft.AspNetCore.Mvc;

namespace HrAi.Api.Controllers;

[ApiController]
[Route("api/ai-performance-reports")]
public class AiPerformanceReportsController : ControllerBase
{
    private readonly IAiPerformanceLogService _performanceLogService;

    public AiPerformanceReportsController(
        IAiPerformanceLogService performanceLogService)
    {
        _performanceLogService = performanceLogService;
    }

    [HttpGet("summary")]
    public async Task<IActionResult> GetSummary(
        [FromQuery] DateTime fromDate,
        [FromQuery] DateTime toDate)
    {
        var result = await _performanceLogService.GetSummaryAsync(
            fromDate,
            toDate);

        return Ok(result);
    }
}