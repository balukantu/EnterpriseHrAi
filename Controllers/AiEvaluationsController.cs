using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/ai-evaluations")]
public class AiEvaluationsController : ControllerBase
{
    private readonly IAiEvaluationService _evaluationService;

    public AiEvaluationsController(IAiEvaluationService evaluationService)
    {
        _evaluationService = evaluationService;
    }

    [HttpPost("run")]
    public async Task<IActionResult> Run()
    {
        var result = await _evaluationService.RunEvaluationAsync();
        return Ok(result);
    }
}