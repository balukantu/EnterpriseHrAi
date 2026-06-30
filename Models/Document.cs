namespace HrAi.Api.Models;

public class Document
{
    public int DocumentId { get; set; }

    public string Title { get; set; } = string.Empty;

    public string DocumentType { get; set; } = string.Empty;

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public ICollection<DocumentChunk> Chunks { get; set; } = new List<DocumentChunk>();
}