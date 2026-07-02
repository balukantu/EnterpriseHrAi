using HrAi.Api.Dtos;
using Microsoft.SemanticKernel;
using Microsoft.SemanticKernel.ChatCompletion;
using Microsoft.SemanticKernel.Connectors.OpenAI;

namespace HrAi.Api.Services;

public class LlmRouterService : ILlmRouterService
{
    private readonly ILlmRoutingPolicyService _routingPolicyService;
    private readonly IConfiguration _configuration;
    private readonly ILogger<LlmRouterService> _logger;

    public LlmRouterService(
        ILlmRoutingPolicyService routingPolicyService,
        IConfiguration configuration,
        ILogger<LlmRouterService> logger)
    {
        _routingPolicyService = routingPolicyService;
        _configuration = configuration;
        _logger = logger;
    }

    public async Task<LlmProviderResultDto> GetChatResponseAsync(
        Kernel kernel,
        ChatHistory chatHistory,
        OpenAIPromptExecutionSettings settings,
        string userMessage,
        CancellationToken cancellationToken = default)
    {
        var selectedServiceId = _routingPolicyService.GetServiceId(userMessage);

        try
        {
            var selectedChatService =
                kernel.GetRequiredService<IChatCompletionService>(selectedServiceId);

            var result = await selectedChatService.GetChatMessageContentAsync(
                chatHistory,
                executionSettings: settings,
                kernel: kernel,
                cancellationToken: cancellationToken);

            return new LlmProviderResultDto
            {
                Answer = result.Content ?? string.Empty,
                ServiceId = selectedServiceId,
                UsedFallback = false
            };
        }
        catch (Exception ex)
        {
            _logger.LogWarning(ex, "Selected LLM failed. Trying fallback provider.");

            var fallbackServiceId = _configuration["LLMSettings:FallbackServiceId"]
                ?? "openai-chat";

            var fallbackChatService =
                kernel.GetRequiredService<IChatCompletionService>(fallbackServiceId);

            var fallbackResult = await fallbackChatService.GetChatMessageContentAsync(
                chatHistory,
                executionSettings: settings,
                kernel: kernel,
                cancellationToken: cancellationToken);

            return new LlmProviderResultDto
            {
                Answer = fallbackResult.Content ?? string.Empty,
                ServiceId = fallbackServiceId,
                UsedFallback = true,
                ErrorMessage = ex.Message
            };
        }
    }
}