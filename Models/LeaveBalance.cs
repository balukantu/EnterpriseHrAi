namespace HrAi.Api.Models;

public class LeaveBalance
{
    public int LeaveBalanceId { get; set; }

    public int EmployeeId { get; set; }

    public string LeaveType { get; set; } = string.Empty;

    public int BalanceDays { get; set; }

    public Employee? Employee { get; set; }
}