public interface IAiCostService
{
    int EstimateTokens(string text);

    decimal CalculateCost(int promptTokens, int completionTokens);
}