namespace HrAi.Api.Services;

public interface IPromptInjectionDetectionService
{
    bool IsSuspicious(string text);
}