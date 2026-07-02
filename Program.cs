using HrAi.Api.Data;
using HrAi.Api.Repositories;
using HrAi.Api.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.SemanticKernel;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

builder.Services.AddDbContext<HrAiDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddScoped<IEmployeeRepository, EmployeeRepository>();
builder.Services.AddScoped<ILeaveRepository, LeaveRepository>();
builder.Services.AddScoped<IChatRepository, ChatRepository>();
builder.Services.AddScoped<IPayrollRepository, PayrollRepository>();

builder.Services.AddScoped<IEmployeeService, EmployeeService>();
builder.Services.AddScoped<ILeaveService, LeaveService>();
builder.Services.AddScoped<IEmployeeProfileService, EmployeeProfileService>();
builder.Services.AddScoped<IPayrollService, PayrollService>();

builder.Services.AddScoped<IPolicySearchRepository, PolicySearchRepository>();
builder.Services.AddScoped<IPolicySearchService, PolicySearchService>();

builder.Services.AddScoped<IPromptBuilder, PromptBuilder>();
builder.Services.AddScoped<IAiOrchestratorService, AiOrchestratorService>();

builder.Services.AddScoped<IEmbeddingService, OpenAiEmbeddingService>();
builder.Services.AddScoped<IVectorPolicySearchRepository, VectorPolicySearchRepository>();
builder.Services.AddScoped<IVectorPolicySearchService, VectorPolicySearchService>();

builder.Services.AddScoped<ITextExtractionService, TextExtractionService>();
builder.Services.AddScoped<ITextChunkingService, TextChunkingService>();
builder.Services.AddScoped<IDocumentIngestionService, DocumentIngestionService>();

builder.Services.AddScoped<IAiInteractionLogRepository, AiInteractionLogRepository>();
builder.Services.AddScoped<IAiInteractionLogService, AiInteractionLogService>();

builder.Services.AddScoped<IPromptVersionRepository, PromptVersionRepository>();
builder.Services.AddScoped<IPromptVersionService, PromptVersionService>();

builder.Services.AddScoped<IAiAuthorizationService, AiAuthorizationService>();

builder.Services.AddMemoryCache();
builder.Services.AddSingleton<ICacheService, MemoryCacheService>();

builder.Services.AddScoped<IAiCostService, AiCostService>();

builder.Services.AddScoped<IAiPerformanceLogRepository, AiPerformanceLogRepository>();
builder.Services.AddScoped<IAiPerformanceLogService, AiPerformanceLogService>();

builder.Services.AddScoped<IAiEvaluationRepository, AiEvaluationRepository>();
builder.Services.AddScoped<IAiEvaluationService, AiEvaluationService>();

builder.Services.AddKernel()
    .AddAzureOpenAIChatCompletion(
        deploymentName: builder.Configuration["AzureOpenAI:DeploymentName"]!,
        endpoint: builder.Configuration["AzureOpenAI:Endpoint"]!,
        apiKey: builder.Configuration["AzureOpenAI:ApiKey"]!
    );

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();

app.UseHttpsRedirection();

app.MapControllers();

app.Run();