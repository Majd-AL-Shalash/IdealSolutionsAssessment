namespace IdealSolutionsAssessment.DTOs;

public class TaskDTO
{
    public required Guid Id { get; set; }
    public required string Title { get; set; }
    public required string Description { get; set; }
    public required string Status { get; set; } = "Pending";
    public required Guid AssignedUserId { get; set; }
}
