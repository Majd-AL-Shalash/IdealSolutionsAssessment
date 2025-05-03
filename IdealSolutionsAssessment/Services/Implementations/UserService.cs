using IdealSolutionsAssessment.DTOs;
using IdealSolutionsAssessment.Models;
using IdealSolutionsAssessment.Repositories.Interfaces;
using IdealSolutionsAssessment.Services.Interfaces;

namespace IdealSolutionsAssessment.Services.Implementations;

public class UserService : IUserService
{
    private readonly IUserRepository _userRepository;

    public UserService(IUserRepository repo)
    {
        _userRepository = repo;
    }

    public async Task<IEnumerable<UserDTO>> GetAllAsync()
    {
        var users = await _userRepository.GetAllAsync();
        return users.Select(usr => new UserDTO
        {
            Id = usr.Id,
            Login = usr.Login,
            Name = usr.Name,
            IsAdmin = usr.IsAdmin,
        });
    }

    public async Task<UserDTO?> GetByIdAsync(Guid id)
    {
        var user = await _userRepository.GetByIdAsync(id);

        return user == null ? null : new UserDTO
        {
            Id = user.Id,
            Login = user.Login,
            Name = user.Name,
            IsAdmin = user.IsAdmin,
        };
    }

    public async Task<UserDTO> CreateAsync(CreateUserDTO dto)
    {
        var user = new User
        {
            Login = dto.Login,
            Name = dto.Name,
            Password = dto.Password,
            IsAdmin = dto.IsAdmin,
        };

        await _userRepository.AddAsync(user);
        await _userRepository.SaveChangesAsync();

        return new UserDTO
        {
            Id = user.Id,
            Login = user.Login,
            Name = user.Name,
            IsAdmin = user.IsAdmin,
        };
    }

    public async Task<bool> UpdateAsync(Guid id, UpdateUserDTO dto)
    {
        var user = await _userRepository.GetByIdAsync(id);
        if (user == null) return false;

        if (!string.IsNullOrWhiteSpace(dto.Name))
            user.Name = dto.Name;

        if (!string.IsNullOrWhiteSpace(dto.Password))
            user.Password = dto.Password;

        if (dto.IsAdmin != null)
            user.IsAdmin = dto.IsAdmin.Value;

        await _userRepository.UpdateAsync(user);
        await _userRepository.SaveChangesAsync();
        return true;
    }

    public async Task<bool> DeleteAsync(Guid id)
    {
        var user = await _userRepository.GetByIdAsync(id);
        if (user == null) return false;

        await _userRepository.DeleteAsync(user);
        await _userRepository.SaveChangesAsync();
        return true;
    }

    public async Task<User?> AuthenticateAsync(string username, string password)
    {
        return await _userRepository.GetByUsernameAndPasswordAsync(username, password);
    }
}
