using HrAi.Api.Dtos;
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
    private readonly IPolicySearchService _policySearchService;
    private readonly IVectorPolicySearchService _vectorPolicySearchService;
    private readonly IAiInteractionLogService _aiInteractionLogService;
    private readonly IPromptVersionService _promptVersionService;

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
        IPromptVersionService promptVersionService)
    {
        _kernel = kernel;
        _leaveService = leaveService;
        _employeeProfileService = employeeProfileService;
        _payrollService = payrollService;
        _policySearchService = policySearchService;
        _chatRepository = chatRepository;
        _promptBuilder = promptBuilder;
        _vectorPolicySearchService = vectorPolicySearchService;
        _aiInteractionLogService = aiInteractionLogService;
        _promptVersionService = promptVersionService;
    }

    public async Task<AiChatResponseDto> ChatAsync(AiChatRequestDto request)
    {
        var stopwatch = Stopwatch.StartNew();

        if (string.IsNullOrWhiteSpace(request.Message))
            throw new ArgumentException("Message is required.");

        var session = await _chatRepository.GetOrCreateSessionAsync(
            request.ChatSessionId,
            request.EmployeeId);

        var previousMessages = await _chatRepository.GetRecentMessagesAsync(
            session.ChatSessionId,
            10);

        var chatHistory = new ChatHistory();

        var systemPrompt = await _promptVersionService.BuildActivePromptAsync(
            "HR_ASSISTANT_SYSTEM_PROMPT",
            request.EmployeeId);

        chatHistory.AddSystemMessage(systemPrompt);

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
                new LeavePlugin(_leaveService),
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
                new PolicySearchPlugin(_policySearchService, _vectorPolicySearchService),
                pluginName: "PolicySearchPlugin");
        }

        var settings = new OpenAIPromptExecutionSettings
        {
            FunctionChoiceBehavior = FunctionChoiceBehavior.Auto()
        };

        var chatCompletionService =
            _kernel.GetRequiredService<IChatCompletionService>();

        var result = await chatCompletionService.GetChatMessageContentAsync(
            chatHistory,
            executionSettings: settings,
            kernel: _kernel);

        var answer = result.Content ?? string.Empty;

        await _chatRepository.AddMessageAsync(
            session.ChatSessionId,
            "user",
            request.Message);

        await _chatRepository.AddMessageAsync(
            session.ChatSessionId,
            "assistant",
            answer);

        stopwatch.Stop();

        await _aiInteractionLogService.SaveAsync(new AiInteractionLog
        {
            LogId = Guid.NewGuid(),
            EmployeeId = request.EmployeeId,
            ChatSessionId = session.ChatSessionId,
            UserQuestion = request.Message,
            PluginsUsed = "Semantic Kernel Auto Function Calling",
            RetrievedDocuments = null,
            SimilarityScores = null,
            Prompt = systemPrompt,
            AiResponse = answer,
            ModelName = "Configured OpenAI Model",
            PromptTokens = null,
            CompletionTokens = null,
            TotalTokens = null,
            ResponseTimeMs = (int)stopwatch.ElapsedMilliseconds,
            Status = "Success",
            ErrorMessage = null,
            CreatedAt = DateTime.UtcNow
        });

        return new AiChatResponseDto
        {
            ChatSessionId = session.ChatSessionId,
            Answer = answer
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
                new LeavePlugin(_leaveService),
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
                new PolicySearchPlugin(_policySearchService, _vectorPolicySearchService),
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