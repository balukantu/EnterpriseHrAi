namespace HrAi.Api.Dtos;

public class AiAgentStepDto
{
    public int StepNumber { get; set; }

    public string StepName { get; set; } = string.Empty;

    public string Result { get; set; } = string.Empty;
}