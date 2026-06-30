namespace HrAi.Api.Models;

public class DocumentChunk
{
    public int DocumentChunkId { get; set; }

    public int DocumentId { get; set; }

    public string ChunkText { get; set; } = string.Empty;

    public string? Embedding { get; set; }

    public int? PageNumber { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public Document? Document { get; set; }
}