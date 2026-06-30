namespace HrAi.Api.Dtos;

public class DocumentUploadResponseDto
{
    public int DocumentId { get; set; }

    public string Title { get; set; } = string.Empty;

    public int ChunkCount { get; set; }

    public string Message { get; set; } = string.Empty;
}