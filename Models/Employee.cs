namespace HrAi.Api.Models;

public class Employee
{
    public int EmployeeId { get; set; }

    public string FirstName { get; set; } = string.Empty;

    public string LastName { get; set; } = string.Empty;

    public string Email { get; set; } = string.Empty;

    public int DepartmentId { get; set; }

    public int? ManagerId { get; set; }

    public DateTime HireDate { get; set; }

    public Department? Department { get; set; }

    public Employee? Manager { get; set; }

    public ICollection<LeaveBalance> LeaveBalances { get; set; } = new List<LeaveBalance>();

    public ICollection<LeaveRequest> LeaveRequests { get; set; } = new List<LeaveRequest>();
}