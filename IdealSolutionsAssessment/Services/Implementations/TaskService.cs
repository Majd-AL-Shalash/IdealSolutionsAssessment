using IdealSolutionsAssessment.DTOs;
using IdealSolutionsAssessment.Enums;
using IdealSolutionsAssessment.Models;
using IdealSolutionsAssessment.Repositories.Interfaces;
using IdealSolutionsAssessment.Services.Interfaces;

namespace IdealSolutionsAssessment.Services.Implementations;

public class TaskService : ITaskService
{
    private readonly ITaskRepository _taskRepository;

    public TaskService(ITaskRepository repo)
    {
        _taskRepository = repo;
    }

    public async Task<IEnumerable<TaskDTO>> GetAllAsync()
    {
        var tasks = await _taskRepository.GetAllAsync();
        return tasks.Select(t => new TaskDTO
        {
            Id = t.Id,
            Title = t.Title,
            Description = t.Description,
            Status = t.Status,
            AssignedUserId = t.AssignedUserId
        });
    }

    public async Task<(GetTaskFlags taskFlags, TaskDTO? task)> GetByIdAsync(Guid id, Guid userId, bool isAdmin)
    {
        var task = await _taskRepository.GetByIdAsync(id);
        if (task == null) return (GetTaskFlags.TaskNotFound, null);

        if (!isAdmin && task.AssignedUserId != userId)
        {
            return (GetTaskFlags.Forbidden, null);
        }

        return (GetTaskFlags.Success, new TaskDTO
        {
            Id = task.Id,
            Title = task.Title,
            Description = task.Description,
            Status = task.Status,
            AssignedUserId = task.AssignedUserId
        });
    }

    public async Task<TaskDTO> CreateAsync(CreateTaskDTO dto)
    {
        var task = new TaskItem
        {
            Title = dto.Title,
            Description = dto.Description,
            Status = "Pending",
            AssignedUserId = dto.AssignedUserId
        };

        await _taskRepository.AddAsync(task);
        await _taskRepository.SaveChangesAsync();

        return new TaskDTO
        {
            Id = task.Id,
            Title = task.Title,
            Description = task.Description,
            Status = task.Status,
            AssignedUserId = task.AssignedUserId
        };
    }
    public async Task<UpdateTaskFlags> UpdateAsync(Guid id, UpdateTaskDTO dto, Guid userId, bool isAdmin)
    {
        var task = await _taskRepository.GetByIdAsync(id);
        if (task == null)
            return UpdateTaskFlags.TaskNotFound;

        if (isAdmin)
        {
            if (!string.IsNullOrWhiteSpace(dto.Title))
                task.Title = dto.Title;

            if (!string.IsNullOrWhiteSpace(dto.Description))
                task.Description = dto.Description;

            if (!string.IsNullOrWhiteSpace(dto.Status))
                task.Status = dto.Status;
        }
        else
        {
            if (task.AssignedUserId != userId)
                return UpdateTaskFlags.Forbidden;

            if (!string.IsNullOrWhiteSpace(dto.Status))
                task.Status = dto.Status;
        }

        await _taskRepository.UpdateAsync(task);
        await _taskRepository.SaveChangesAsync();

        return UpdateTaskFlags.Success;
    }

    public async Task<bool> DeleteAsync(Guid id)
    {
        var task = await _taskRepository.GetByIdAsync(id);
        if (task == null) return false;

        await _taskRepository.DeleteAsync(task);
        await _taskRepository.SaveChangesAsync();
        return true;
    }
}
