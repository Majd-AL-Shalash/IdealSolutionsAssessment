namespace IdealSolutionsAssessment.Models;

public class User
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public string Login { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
    public bool IsAdmin { get; set; } = false;

    public ICollection<TaskItem> Tasks { get; set; } = [];
}
