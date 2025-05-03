namespace IdealSolutionsAssessment.DTOs;

public class CreateUserDTO
{
    public required string Login { get; set; }
    public required string Name { get; set; }
    public required string Password { get; set; }
    public required bool IsAdmin { get; set; }
}
