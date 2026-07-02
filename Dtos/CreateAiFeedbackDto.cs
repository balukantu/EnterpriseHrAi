namespace HrAi.Api.Dtos;

public class CreateAiFeedbackDto
{
    public Guid LogId { get; set; }

    public int EmployeeId { get; set; }

    public int Rating { get; set; }

    public string? Comment { get; set; }
}