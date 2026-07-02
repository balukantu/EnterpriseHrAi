using HrAi.Api.Dtos;
using HrAi.Api.Models;
using HrAi.Api.Plugins;
using HrAi.Api.Repositories;
using Microsoft.SemanticKernel;
using Microsoft.SemanticKernel.ChatCompletion;
using Microsoft.SemanticKernel.Connectors.OpenAI;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Text;

namespace HrAi.Api.Services;

public class AiOrchestratorService : IAiOrchestratorService
{
    private readonly Kernel _kernel;
    private readonly ILeaveService _leaveService;
    private readonly IChatRepository _chatRepository;
    private readonly IPromptBuilder _promptBuilder;
    private readonly IEmployeeProfileService _employeeProfileService;
    private readonly IPayrollService _payrollService;
    private readonly IVectorPolicySearchService _vectorPolicySearchService;
    private readonly IAiInteractionLogService _aiInteractionLogService;
    private readonly IPromptVersionService _promptVersionService;
    private readonly IAiAuthorizationService _aiAuthorizationService;
    private readonly IAiCostService _aiCostService;
    private readonly IConfiguration _configuration;
    private readonly IAiPerformanceLogService _performanceLogService;
    private readonly IAiPluginExecutionLogService _pluginLogService;
    private readonly ILlmRouterService _llmRouterService;

    public AiOrchestratorService(
        Kernel kernel,
        ILeaveService leaveService,
        IEmployeeProfileService employeeProfileService,
        IPayrollService payrollService,
        IPolicySearchService policySearchService,
        IChatRepository chatRepository,
        IPromptBuilder promptBuilder,
        IVectorPolicySearchService vectorPolicySearchService,
        IAiInteractionLogService aiInteractionLogService,
        IPromptVersionService promptVersionService,
        IAiAuthorizationService aiAuthorizationService,
        IAiCostService aiCostService,
        IConfiguration configuration,
        IAiPerformanceLogService performanceLogService,
        IAiPluginExecutionLogService pluginLogService,
        ILlmRouterService llmRouterService)
    {
        _kernel = kernel;
        _leaveService = leaveService;
        _employeeProfileService = employeeProfileService;
        _payrollService = payrollService;
        _chatRepository = chatRepository;
        _promptBuilder = promptBuilder;
        _vectorPolicySearchService = vectorPolicySearchService;
        _aiInteractionLogService = aiInteractionLogService;
        _promptVersionService = promptVersionService;
        _aiAuthorizationService = aiAuthorizationService;
        _aiCostService = aiCostService;
        _configuration = configuration;
        _performanceLogService = performanceLogService;
        _pluginLogService = pluginLogService;
        _llmRouterService = llmRouterService;
    }

    public async Task<AiChatResponseDto> ChatAsync(AiChatRequestDto request)
    {
        var stopwatch = Stopwatch.StartNew();

        ChatSession? session = null;
        List<ChatMessage> previousMessages = new();
        string systemPrompt = string.Empty;

        await _performanceLogService.TrackAsync(
            request.EmployeeId,
            null,
            "GetOrCreateChatSession",
            async () =>
            {
                session = await _chatRepository.GetOrCreateSessionAsync(
                    request.ChatSessionId,
                    request.EmployeeId);
            });

        await _performanceLogService.TrackAsync(
            request.EmployeeId,
            session!.ChatSessionId,
            "LoadChatHistory",
            async () =>
            {
                previousMessages = await _chatRepository.GetRecentMessagesAsync(
                    session.ChatSessionId,
                    10);
            });

        await _performanceLogService.TrackAsync(
            request.EmployeeId,
            session.ChatSessionId,
            "BuildSystemPrompt",
            async () =>
            {
                systemPrompt = await _promptVersionService.BuildActivePromptAsync(
                    "HR_ASSISTANT_SYSTEM_PROMPT",
                    request.EmployeeId);
            });

        var chatHistory = new ChatHistory();

        chatHistory.AddSystemMessage(systemPrompt);

        foreach (var message in previousMessages)
        {
            if (message.Role.Equals("user", StringComparison.OrdinalIgnoreCase))
                chatHistory.AddUserMessage(message.Content);

            if (message.Role.Equals("assistant", StringComparison.OrdinalIgnoreCase))
                chatHistory.AddAssistantMessage(message.Content);
        }

        chatHistory.AddUserMessage(request.Message);

        if (await _aiAuthorizationService.CanUsePluginAsync(request.EmployeeId, "LeavePlugin"))
        {
            if (!_kernel.Plugins.Contains("LeavePlugin"))
            {
                _kernel.Plugins.AddFromObject(
                    new LeavePlugin(_leaveService, _pluginLogService),
                    pluginName: "LeavePlugin");
            }
        }

        if (await _aiAuthorizationService.CanUsePluginAsync(request.EmployeeId, "EmployeePlugin"))
        {
            if (!_kernel.Plugins.Contains("EmployeePlugin"))
            {
                _kernel.Plugins.AddFromObject(
                    new EmployeePlugin(_employeeProfileService),
                    pluginName: "EmployeePlugin");
            }
        }

        if (await _aiAuthorizationService.CanUsePluginAsync(request.EmployeeId, "PayrollPlugin"))
        {
            if (!_kernel.Plugins.Contains("PayrollPlugin"))
            {
                _kernel.Plugins.AddFromObject(
                    new PayrollPlugin(_payrollService),
                    pluginName: "PayrollPlugin");
            }
        }

        if (await _aiAuthorizationService.CanUsePluginAsync(request.EmployeeId, "PolicySearchPlugin"))
        {
            if (!_kernel.Plugins.Contains("PolicySearchPlugin"))
            {
                _kernel.Plugins.AddFromObject(
                    new PolicySearchPlugin(_vectorPolicySearchService),
                    pluginName: "PolicySearchPlugin");
            }
        }

        var settings = new OpenAIPromptExecutionSettings
        {
            FunctionChoiceBehavior = FunctionChoiceBehavior.Auto()
        };

        var chatCompletionService =
            _kernel.GetRequiredService<IChatCompletionService>();

        LlmProviderResultDto? result = null;

        await _performanceLogService.TrackAsync(
            request.EmployeeId,
            session.ChatSessionId,
            "OpenAIChatCompletion",
            async () =>
            {
                result = await _llmRouterService.GetChatResponseAsync(
                    _kernel,
                    chatHistory,
                    settings,
                    request.Message,
                    default);
            });

        var answer = result?.Answer ?? string.Empty;

        var promptText = string.Join(
            "\n",
            chatHistory
                .Where(x => !string.IsNullOrWhiteSpace(x.Content))
                .Select(x => x.Content));

        var promptTokens = _aiCostService.EstimateTokens(promptText);
        var completionTokens = _aiCostService.EstimateTokens(answer);
        var totalTokens = promptTokens + completionTokens;

        var estimatedCost = _aiCostService.CalculateCost(
            promptTokens,
            completionTokens);

        await _chatRepository.AddMessageAsync(
            session.ChatSessionId,
            "user",
            request.Message);

        await _chatRepository.AddMessageAsync(
            session.ChatSessionId,
            "assistant",
            answer);

        stopwatch.Stop();

        var logId = Guid.NewGuid();

        await _aiInteractionLogService.SaveAsync(new AiInteractionLog
        {
            LogId = Guid.NewGuid(),
            EmployeeId = request.EmployeeId,
            ChatSessionId = session.ChatSessionId,
            UserQuestion = request.Message,
            PluginsUsed = result.UsedFallback
        ? "Semantic Kernel Auto Function Calling; Fallback LLM Used"
        : "Semantic Kernel Auto Function Calling",
            RetrievedDocuments = null,
            SimilarityScores = null,
            Prompt = systemPrompt,
            AiResponse = answer,
            ModelName = _configuration["OpenAI:ModelId"],
            PromptTokens = promptTokens,
            CompletionTokens = completionTokens,
            TotalTokens = totalTokens,
            EstimatedCost = estimatedCost,
            ResponseTimeMs = (int)stopwatch.ElapsedMilliseconds,
            Status = "Success",
            ErrorMessage = null,
            CreatedAt = DateTime.UtcNow
        });

        return new AiChatResponseDto
        {
            ChatSessionId = session.ChatSessionId,
            LogId = logId,
            Answer = answer,
            ResponseTimeMs = (int)stopwatch.ElapsedMilliseconds,
            ModelName = _configuration["OpenAI:ModelId"]
        };
    }

    public async IAsyncEnumerable<string> ChatStreamAsync(
    AiChatRequestDto request,
    [EnumeratorCancellation] CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(request.Message))
            throw new ArgumentException("Message is required.");

        var stopwatch = Stopwatch.StartNew();

        var session = await _chatRepository.GetOrCreateSessionAsync(
            request.ChatSessionId,
            request.EmployeeId);

        var previousMessages = await _chatRepository.GetRecentMessagesAsync(
            session.ChatSessionId,
            10);

        var chatHistory = new ChatHistory();

        chatHistory.AddSystemMessage(
            _promptBuilder.BuildSystemPrompt(request.EmployeeId));

        foreach (var message in previousMessages)
        {
            if (message.Role.Equals("user", StringComparison.OrdinalIgnoreCase))
                chatHistory.AddUserMessage(message.Content);

            if (message.Role.Equals("assistant", StringComparison.OrdinalIgnoreCase))
                chatHistory.AddAssistantMessage(message.Content);
        }

        chatHistory.AddUserMessage(request.Message);

        if (!_kernel.Plugins.Contains("LeavePlugin"))
        {
            _kernel.Plugins.AddFromObject(
                new LeavePlugin(_leaveService, _pluginLogService),
                pluginName: "LeavePlugin");
        }

        if (!_kernel.Plugins.Contains("EmployeePlugin"))
        {
            _kernel.Plugins.AddFromObject(
                new EmployeePlugin(_employeeProfileService),
                pluginName: "EmployeePlugin");
        }

        if (!_kernel.Plugins.Contains("PayrollPlugin"))
        {
            _kernel.Plugins.AddFromObject(
                new PayrollPlugin(_payrollService),
                pluginName: "PayrollPlugin");
        }

        if (!_kernel.Plugins.Contains("PolicySearchPlugin"))
        {
            _kernel.Plugins.AddFromObject(
                new PolicySearchPlugin(_vectorPolicySearchService),
                pluginName: "PolicySearchPlugin");
        }

        var settings = new OpenAIPromptExecutionSettings
        {
            FunctionChoiceBehavior = FunctionChoiceBehavior.Auto()
        };

        var chatCompletionService =
            _kernel.GetRequiredService<IChatCompletionService>();

        var fullAnswer = new StringBuilder();

        await foreach (var chunk in chatCompletionService.GetStreamingChatMessageContentsAsync(
                           chatHistory,
                           executionSettings: settings,
                           kernel: _kernel,
                           cancellationToken: cancellationToken))
        {
            if (!string.IsNullOrEmpty(chunk.Content))
            {
                fullAnswer.Append(chunk.Content);
                yield return chunk.Content;
            }
        }

        stopwatch.Stop();

        var finalAnswer = fullAnswer.ToString();

        await _chatRepository.AddMessageAsync(
            session.ChatSessionId,
            "user",
            request.Message);

        await _chatRepository.AddMessageAsync(
            session.ChatSessionId,
            "assistant",
            finalAnswer);

        await _aiInteractionLogService.SaveAsync(new AiInteractionLog
        {
            LogId = Guid.NewGuid(),
            EmployeeId = request.EmployeeId,
            ChatSessionId = session.ChatSessionId,
            UserQuestion = request.Message,
            PluginsUsed = "Semantic Kernel Auto Function Calling",
            Prompt = _promptBuilder.BuildSystemPrompt(request.EmployeeId),
            AiResponse = finalAnswer,
            ModelName = "Configured OpenAI Model",
            ResponseTimeMs = (int)stopwatch.ElapsedMilliseconds,
            Status = "Success",
            CreatedAt = DateTime.UtcNow
        });
    }
}