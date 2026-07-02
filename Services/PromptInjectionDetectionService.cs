namespace HrAi.Api.Services;

public class PromptInjectionDetectionService : IPromptInjectionDetectionService
{
    private static readonly string[] SuspiciousPatterns =
    {
        "ignore previous instructions",
        "ignore all previous instructions",
        "forget your instructions",
        "you are now",
        "system prompt",
        "developer message",
        "reveal prompt",
        "show hidden instructions",
        "bypass authorization",
        "call payrollplugin",
        "call employeeplugin",
        "show all salaries",
        "delete records",
        "drop table"
    };

    public bool IsSuspicious(string text)
    {
        if (string.IsNullOrWhiteSpace(text))
            return false;

        var normalized = text.ToLowerInvariant();

        return SuspiciousPatterns.Any(pattern =>
            normalized.Contains(pattern));
    }
}