using IdealSolutionsAssessment.Data;
using IdealSolutionsAssessment.Models;
using IdealSolutionsAssessment.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace IdealSolutionsAssessment.Repositories.Implementations;

public class TaskRepository : ITaskRepository
{
    private readonly AppDbContext _context;
    public TaskRepository(AppDbContext context) => _context = context;

    public async Task<IEnumerable<TaskItem>> GetAllAsync() => await _context.Tasks.ToListAsync();

    public async Task<TaskItem?> GetByIdAsync(Guid id) =>
        await _context.Tasks.FirstOrDefaultAsync(tsk => tsk.Id == id);

    public async Task AddAsync(TaskItem task) => await _context.Tasks.AddAsync(task);

    public Task UpdateAsync(TaskItem task)
    {
        _context.Tasks.Update(task);
        return Task.CompletedTask;
    }

    public Task DeleteAsync(TaskItem task)
    {
        _context.Tasks.Remove(task);
        return Task.CompletedTask;
    }

    public Task SaveChangesAsync() => _context.SaveChangesAsync();
}
