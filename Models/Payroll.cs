namespace HrAi.Api.Models;

public class Payroll
{
    public int PayrollId { get; set; }

    public int EmployeeId { get; set; }

    public DateTime PayMonth { get; set; }

    public decimal GrossSalary { get; set; }

    public decimal TaxAmount { get; set; }

    public decimal NetSalary { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public Employee? Employee { get; set; }
}