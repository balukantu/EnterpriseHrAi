namespace HrAi.Api.Services;

public interface IPromptBuilder
{
    string BuildSystemPrompt(int employeeId);
}