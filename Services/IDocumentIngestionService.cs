using HrAi.Api.Dtos;

namespace HrAi.Api.Services;

public interface IDocumentIngestionService
{
    Task<DocumentUploadResponseDto> UploadDocumentAsync(IFormFile file, string documentType);
}