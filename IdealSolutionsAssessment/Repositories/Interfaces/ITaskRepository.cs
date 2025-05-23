﻿using IdealSolutionsAssessment.Models;

namespace IdealSolutionsAssessment.Repositories.Interfaces;

public interface ITaskRepository
{
    Task<IEnumerable<TaskItem>> GetAllAsync();
    Task<TaskItem?> GetByIdAsync(Guid id);
    Task AddAsync(TaskItem task);
    Task UpdateAsync(TaskItem task);
    Task DeleteAsync(TaskItem task);
    Task SaveChangesAsync();
}
