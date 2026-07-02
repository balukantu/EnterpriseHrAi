namespace HrAi.Api.Services;

public class LlmRoutingPolicyService : ILlmRoutingPolicyService
{
    public string GetServiceId(string userMessage)
    {
        var message = userMessage.ToLowerInvariant();

        if (message.Contains("summary") ||
            message.Contains("summarize"))
        {
            return "openai-chat";
        }

        if (message.Contains("payroll") ||
            message.Contains("salary") ||
            message.Contains("tax") ||
            message.Contains("leave") ||
            message.Contains("policy") ||
            message.Contains("work from home") ||
            message.Contains("remote work"))
        {
            return "azure-openai-chat";
        }

        return "azure-openai-chat";
    }
}