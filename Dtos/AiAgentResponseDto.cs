namespace HrAi.Api.Dtos;

public class AiAgentResponseDto
{
    public string FinalAnswer { get; set; } = string.Empty;

    public List<AiAgentStepDto> Steps { get; set; } = new();
}