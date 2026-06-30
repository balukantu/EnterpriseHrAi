namespace HrAi.Api.Services;

public interface ITextExtractionService
{
    Task<string> ExtractTextAsync(IFormFile file);
}