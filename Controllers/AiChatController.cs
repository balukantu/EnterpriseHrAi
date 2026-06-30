using HrAi.Api.Dtos;
using HrAi.Api.Services;
using Microsoft.AspNetCore.Mvc;

namespace HrAi.Api.Controllers;

[ApiController]
[Route("api/ai-chat")]
public class AiChatController : ControllerBase
{
    private readonly IAiOrchestratorService _aiOrchestratorService;

    public AiChatController(IAiOrchestratorService aiOrchestratorService)
    {
        _aiOrchestratorService = aiOrchestratorService;
    }

    [HttpPost]
    public async Task<IActionResult> Chat([FromBody] AiChatRequestDto request)
    {
        var response = await _aiOrchestratorService.ChatAsync(request);
        return Ok(response);
    }

    [HttpPost("stream")]
    public async Task Stream(
    [FromBody] AiChatRequestDto request,
    CancellationToken cancellationToken)
    {
        Response.ContentType = "text/event-stream";
        Response.Headers.CacheControl = "no-cache";
        Response.Headers.Connection = "keep-alive";

        await foreach (var token in _aiOrchestratorService.ChatStreamAsync(
                           request,
                           cancellationToken))
        {
            await Response.WriteAsync($"data: {token}\n\n", cancellationToken);
            await Response.Body.FlushAsync(cancellationToken);
        }

        await Response.WriteAsync("data: [DONE]\n\n", cancellationToken);
        await Response.Body.FlushAsync(cancellationToken);
    }
}