namespace HrAi.Api.Services;

public class PayrollAgent : IAiAgent
{
    private readonly IPayrollService _payrollService;

    public PayrollAgent(IPayrollService payrollService)
    {
        _payrollService = payrollService;
    }

    public async Task<string> HandleAsync(int employeeId, string message)
    {
        var payroll = await _payrollService.GetLatestPayrollAsync(employeeId);

        return $"""
        Payroll Agent Result:

        {payroll}
        """;
    }
}