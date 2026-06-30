namespace HrAi.Api.Services;

public class TextChunkingService : ITextChunkingService
{
    public List<string> ChunkText(string text, int maxChunkSize = 800)
    {
        var chunks = new List<string>();

        if (string.IsNullOrWhiteSpace(text))
            return chunks;

        text = text.Replace("\r\n", "\n").Trim();

        var paragraphs = text
            .Split("\n\n", StringSplitOptions.RemoveEmptyEntries)
            .Select(x => x.Trim())
            .Where(x => !string.IsNullOrWhiteSpace(x))
            .ToList();

        var currentChunk = "";

        foreach (var paragraph in paragraphs)
        {
            if ((currentChunk.Length + paragraph.Length) <= maxChunkSize)
            {
                currentChunk += paragraph + "\n\n";
            }
            else
            {
                if (!string.IsNullOrWhiteSpace(currentChunk))
                    chunks.Add(currentChunk.Trim());

                currentChunk = paragraph + "\n\n";
            }
        }

        if (!string.IsNullOrWhiteSpace(currentChunk))
            chunks.Add(currentChunk.Trim());

        return chunks;
    }
}