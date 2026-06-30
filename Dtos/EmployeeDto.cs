namespace HrAi.Api.Dtos;

public class EmployeeDto
{
    public int EmployeeId { get; set; }

    public string FullName { get; set; } = string.Empty;

    public string Email { get; set; } = string.Empty;

    public string Department { get; set; } = string.Empty;

    public string? ManagerName { get; set; }

    public DateTime HireDate { get; set; }
}