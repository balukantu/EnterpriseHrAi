using HrAi.Api.Services;
using Microsoft.AspNetCore.Mvc;

namespace HrAi.Api.Controllers;

[ApiController]
[Route("api/documents")]
public class DocumentsController : ControllerBase
{
    private readonly IDocumentIngestionService _documentIngestionService;

    public DocumentsController(IDocumentIngestionService documentIngestionService)
    {
        _documentIngestionService = documentIngestionService;
    }

    [HttpPost("upload")]
    [RequestSizeLimit(10_000_000)]
    public async Task<IActionResult> UploadDocument(
        IFormFile file,
        [FromForm] string documentType = "Policy")
    {
        if (file == null || file.Length == 0)
            return BadRequest("File is required.");

        var result = await _documentIngestionService.UploadDocumentAsync(
            file,
            documentType);

        return Ok(result);
    }
}