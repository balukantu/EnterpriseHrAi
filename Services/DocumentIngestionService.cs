using System.Text.Json;
using HrAi.Api.Data;
using HrAi.Api.Dtos;
using HrAi.Api.Models;

namespace HrAi.Api.Services;

public class DocumentIngestionService : IDocumentIngestionService
{
    private readonly HrAiDbContext _context;
    private readonly ITextExtractionService _textExtractionService;
    private readonly ITextChunkingService _textChunkingService;
    private readonly IEmbeddingService _embeddingService;

    public DocumentIngestionService(
        HrAiDbContext context,
        ITextExtractionService textExtractionService,
        ITextChunkingService textChunkingService,
        IEmbeddingService embeddingService)
    {
        _context = context;
        _textExtractionService = textExtractionService;
        _textChunkingService = textChunkingService;
        _embeddingService = embeddingService;
    }

    public async Task<DocumentUploadResponseDto> UploadDocumentAsync(
        IFormFile file,
        string documentType)
    {
        var text = await _textExtractionService.ExtractTextAsync(file);

        var chunks = _textChunkingService.ChunkText(text);

        if (chunks.Count == 0)
            throw new InvalidOperationException("No readable text found in document.");

        var document = new Document
        {
            Title = Path.GetFileNameWithoutExtension(file.FileName),
            DocumentType = documentType,
            CreatedAt = DateTime.UtcNow
        };

        await _context.Documents.AddAsync(document);
        await _context.SaveChangesAsync();

        foreach (var chunk in chunks)
        {
            var embedding = await _embeddingService.GenerateEmbeddingAsync(chunk);

            var documentChunk = new DocumentChunk
            {
                DocumentId = document.DocumentId,
                ChunkText = chunk,
                Embedding = JsonSerializer.Serialize(embedding),
                PageNumber = null,
                CreatedAt = DateTime.UtcNow
            };

            await _context.DocumentChunks.AddAsync(documentChunk);
        }

        await _context.SaveChangesAsync();

        return new DocumentUploadResponseDto
        {
            DocumentId = document.DocumentId,
            Title = document.Title,
            ChunkCount = chunks.Count,
            Message = "Document uploaded, chunked, embedded, and saved successfully."
        };
    }
}