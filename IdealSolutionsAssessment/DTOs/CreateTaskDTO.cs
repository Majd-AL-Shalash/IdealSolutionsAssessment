namespace IdealSolutionsAssessment.DTOs;

public class CreateTaskDTO
{
    public required string Title { get; set; }
    public required string Description { get; set; }
    public required Guid AssignedUserId { get; set; }
}
