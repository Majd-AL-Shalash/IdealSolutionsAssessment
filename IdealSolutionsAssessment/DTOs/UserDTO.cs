namespace IdealSolutionsAssessment.DTOs;

public class UserDTO
{
    public required Guid Id { get; set; }
    public required string Login { get; set; }
    public required string Name { get; set; }
    public required bool IsAdmin { get; set; }
}
