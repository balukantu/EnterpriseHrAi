using HrAi.Api.Dtos;
using Microsoft.SemanticKernel;
using Microsoft.SemanticKernel.ChatCompletion;
using Microsoft.SemanticKernel.Connectors.OpenAI;

namespace HrAi.Api.Services;

public interface ILlmRouterService
{
    Task<LlmProviderResultDto> GetChatResponseAsync(
        Kernel kernel,
        ChatHistory chatHistory,
        OpenAIPromptExecutionSettings settings,
        string userMessage,
        CancellationToken cancellationToken = default);
}