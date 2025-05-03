using IdealSolutionsAssessment.Models;

namespace IdealSolutionsAssessment.Repositories.Interfaces;

public interface IUserRepository
{
    Task<IEnumerable<User>> GetAllAsync();
    Task<User?> GetByIdAsync(Guid id);
    Task<User?> GetByUsernameAsync(string username);
    Task AddAsync(User user);
    Task UpdateAsync(User user);
    Task DeleteAsync(User user);
    Task<User?> GetByUsernameAndPasswordAsync(string username, string password);
    Task SaveChangesAsync();
}
