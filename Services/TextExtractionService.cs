using System.Text;

namespace HrAi.Api.Services;

public class TextExtractionService : ITextExtractionService
{
    public async Task<string> ExtractTextAsync(IFormFile file)
    {
        if (file == null || file.Length == 0)
            throw new ArgumentException("File is empty.");

        var extension = Path.GetExtension(file.FileName).ToLowerInvariant();

        if (extension != ".txt")
            throw new NotSupportedException("Only .txt files are supported in Module 7.");

        using var reader = new StreamReader(file.OpenReadStream(), Encoding.UTF8);
        return await reader.ReadToEndAsync();
    }
}