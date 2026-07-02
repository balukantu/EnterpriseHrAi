namespace HrAi.Api.Services;

public class AiCostService : IAiCostService
{
    private readonly IConfiguration _configuration;

    public AiCostService(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public int EstimateTokens(string text)
    {
        if (string.IsNullOrWhiteSpace(text))
            return 0;

        // Simple estimate: 1 token ≈ 4 characters
        return (int)Math.Ceiling(text.Length / 4.0);
    }

    public decimal CalculateCost(int promptTokens, int completionTokens)
    {
        var inputCostPerMillion = _configuration.GetValue<decimal>(
            "AiCostSettings:InputCostPerMillionTokens");

        var outputCostPerMillion = _configuration.GetValue<decimal>(
            "AiCostSettings:OutputCostPerMillionTokens");

        var inputCost = promptTokens / 1_000_000m * inputCostPerMillion;
        var outputCost = completionTokens / 1_000_000m * outputCostPerMillion;

        return inputCost + outputCost;
    }
}