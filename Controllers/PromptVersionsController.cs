using HrAi.Api.Dtos;
using HrAi.Api.Services;
using Microsoft.AspNetCore.Mvc;

namespace HrAi.Api.Controllers;

[ApiController]
[Route("api/prompt-versions")]
public class PromptVersionsController : ControllerBase
{
    private readonly IPromptVersionService _promptVersionService;

    public PromptVersionsController(IPromptVersionService promptVersionService)
    {
        _promptVersionService = promptVersionService;
    }

    [HttpGet("{promptName}")]
    public async Task<IActionResult> GetPromptVersions(string promptName)
    {
        var prompts = await _promptVersionService.GetPromptVersionsAsync(promptName);
        return Ok(prompts);
    }

    [HttpPost]
    public async Task<IActionResult> CreatePromptVersion(
        [FromBody] CreatePromptVersionDto dto)
    {
        var result = await _promptVersionService.CreateAsync(dto);
        return Ok(result);
    }

    [HttpPost("{promptVersionId:int}/activate")]
    public async Task<IActionResult> ActivatePromptVersion(int promptVersionId)
    {
        await _promptVersionService.ActivateAsync(promptVersionId);
        return Ok(new { message = "Prompt version activated successfully." });
    }
}