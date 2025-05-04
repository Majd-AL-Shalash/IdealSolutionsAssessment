using IdealSolutionsAssessment.Data;
using IdealSolutionsAssessment.Models;
using IdealSolutionsAssessment.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace IdealSolutionsAssessment.Repositories.Implementations;

public class UserRepository : IUserRepository
{
    private readonly AppDbContext _context;
    public UserRepository(AppDbContext context) => _context = context;

    public async Task<IEnumerable<User>> GetAllAsync() => await _context.Users.ToListAsync();

    public async Task<User?> GetByIdAsync(Guid id) => await _context.Users.FindAsync(id);

    public async Task<User?> GetByUsernameAsync(string username) =>
        await _context.Users.FirstOrDefaultAsync(usr => usr.Name == username);

    public async Task AddAsync(User user) => await _context.Users.AddAsync(user);

    public Task UpdateAsync(User user)
    {
        _context.Users.Update(user);
        return Task.CompletedTask;
    }

    public async Task DeleteAsync(User user)
    {
        var taskToBeDeleted = await _context.Tasks.Where(tsk => tsk.AssignedUserId == user.Id).ToListAsync();

        _context.Users.Remove(user);

        _context.Tasks.RemoveRange(taskToBeDeleted);
    }

    public async Task<User?> GetByUsernameAndPasswordAsync(string username, string password)
    {
        return await _context.Users
            .FirstOrDefaultAsync(usr => usr.Name.ToLower() == username.ToLower() &&
                usr.Password == password);
    }

    public Task SaveChangesAsync() => _context.SaveChangesAsync();
}
