using IdealSolutionsAssessment.Models;
using IdealSolutionsAssessment.Repositories.Interfaces;

namespace IdealSolutionsAssessment.Data;

public class DatabaseSeeder
{
    private readonly IUserRepository _userRepository;
    private readonly ITaskRepository _taskRepository;

    public DatabaseSeeder(IUserRepository userRepository, ITaskRepository taskRepository)
    {
        _userRepository = userRepository;
        _taskRepository = taskRepository;
    }

    public async Task SeedAsync()
    {
        var adminUser = new User
        {
            Id = Guid.NewGuid(),
            Login = "admin",
            Name = "admin",
            Password = "123456",
            IsAdmin = true,
        };

        var regularUser = new User
        {
            Id = Guid.NewGuid(),
            Login = "user",
            Name = "user",
            Password = "123456",
            IsAdmin = false,
        };

        await _userRepository.AddAsync(adminUser);
        await _userRepository.AddAsync(regularUser);
        await _userRepository.SaveChangesAsync();

        var task1 = new TaskItem
        {
            Id = Guid.NewGuid(),
            Title = "Admin Task 1",
            Description = "This is a task for admin.",
            Status = "Pending",
            AssignedUserId = adminUser.Id,
        };

        var task2 = new TaskItem
        {
            Id = Guid.NewGuid(),
            Title = "User Task 1",
            Description = "This is a task accessible by admin and user.",
            Status = "Pending",
            AssignedUserId = regularUser.Id,
        };

        var task3 = new TaskItem
        {
            Id = Guid.NewGuid(),
            Title = "User Task 2",
            Description = "This is a task accessible by admin and user.",
            Status = "Pending",
            AssignedUserId = regularUser.Id,
        };

        await _taskRepository.AddAsync(task1);
        await _taskRepository.AddAsync(task2);
        await _taskRepository.AddAsync(task3);
        await _taskRepository.SaveChangesAsync();
    }
}