using HrAi.Api.Services;
using Microsoft.AspNetCore.Mvc;

namespace HrAi.Api.Controllers;

[ApiController]
[Route("api/embeddings")]
public class EmbeddingsController : ControllerBase
{
    private readonly IVectorPolicySearchService _vectorPolicySearchService;

    public EmbeddingsController(IVectorPolicySearchService vectorPolicySearchService)
    {
        _vectorPolicySearchService = vectorPolicySearchService;
    }

    [HttpPost("generate-missing")]
    public async Task<IActionResult> GenerateMissingEmbeddings()
    {
        var updatedChunkIds = await _vectorPolicySearchService.GenerateMissingEmbeddingsAsync();

        return Ok(new
        {
            message = "Embeddings generated successfully.",
            updatedChunkIds
        });
    }
}