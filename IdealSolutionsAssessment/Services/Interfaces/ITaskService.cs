using IdealSolutionsAssessment.DTOs;
using IdealSolutionsAssessment.Enums;

namespace IdealSolutionsAssessment.Services.Interfaces;

public interface ITaskService
{
    Task<IEnumerable<TaskDTO>> GetAllAsync();
    Task<(GetTaskFlags taskFlags, TaskDTO? task)> GetByIdAsync(Guid id, Guid userId, bool isAdmin);
    Task<TaskDTO> CreateAsync(CreateTaskDTO dto);
    Task<UpdateTaskFlags> UpdateAsync(Guid id, UpdateTaskDTO dto, Guid userId, bool isAdmin);
    Task<bool> DeleteAsync(Guid id);
}
