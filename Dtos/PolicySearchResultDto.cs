namespace HrAi.Api.Dtos;

public class PolicySearchResultDto
{
    public string DocumentTitle { get; set; } = string.Empty;

    public int? PageNumber { get; set; }

    public string Content { get; set; } = string.Empty;
}