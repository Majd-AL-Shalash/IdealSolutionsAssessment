using IdealSolutionsAssessment.DTOs;
using IdealSolutionsAssessment.Models;

namespace IdealSolutionsAssessment.Services.Interfaces;

public interface IUserService
{
    Task<IEnumerable<UserDTO>> GetAllAsync();
    Task<UserDTO?> GetByIdAsync(Guid id);
    Task<UserDTO> CreateAsync(CreateUserDTO dto);
    Task<bool> UpdateAsync(Guid id, UpdateUserDTO dto);
    Task<bool> DeleteAsync(Guid id);
    Task<User?> AuthenticateAsync(string username, string password);
}
