using HrAi.Api.Dtos;
using HrAi.Api.Services;
using Microsoft.AspNetCore.Mvc;

namespace HrAi.Api.Controllers;

[ApiController]
[Route("api/ai-feedback")]
public class AiFeedbackController : ControllerBase
{
    private readonly IAiFeedbackService _feedbackService;

    public AiFeedbackController(IAiFeedbackService feedbackService)
    {
        _feedbackService = feedbackService;
    }

    [HttpPost]
    public async Task<IActionResult> SubmitFeedback(
        [FromBody] CreateAiFeedbackDto dto)
    {
        await _feedbackService.SaveAsync(dto);

        return Ok(new
        {
            message = "Feedback submitted successfully."
        });
    }

    [HttpGet("summary")]
    public async Task<IActionResult> GetSummary(
        [FromQuery] DateTime fromDate,
        [FromQuery] DateTime toDate)
    {
        var result = await _feedbackService.GetSummaryAsync(
            fromDate,
            toDate);

        return Ok(result);
    }
}