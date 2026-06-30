namespace HrAi.Api.Dtos;

public class PayrollDto
{
    public int PayrollId { get; set; }

    public int EmployeeId { get; set; }

    public DateTime PayMonth { get; set; }

    public decimal GrossSalary { get; set; }

    public decimal TaxAmount { get; set; }

    public decimal NetSalary { get; set; }
}