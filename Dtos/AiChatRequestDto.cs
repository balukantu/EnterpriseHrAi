namespace HrAi.Api.Dtos;

public class AiChatRequestDto
{
    public int EmployeeId { get; set; }

    public Guid? ChatSessionId { get; set; }

    public string Message { get; set; } = string.Empty;
}