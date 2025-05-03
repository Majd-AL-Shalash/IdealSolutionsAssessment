using IdealSolutionsAssessment.DTOs;
using IdealSolutionsAssessment.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using IdealSolutionsAssessment.Helpers;

namespace IdealSolutionsAssessment.Controllers;

[Route("api/[controller]")]
[ApiController]
[Authorize]
public class UserController : ControllerBase
{
    private readonly IUserService _userService;

    public UserController(IUserService userService)
    {
        _userService = userService;
    }

    [HttpGet]
    [Authorize(Roles = "Admin")]
    public async Task<ActionResult<IEnumerable<UserDTO>>> GetUsers()
    {
        var users = await _userService.GetAllAsync();
        return Ok(users);
    }

    [HttpGet("me")]
    public async Task<ActionResult<UserDTO>> GetCurrentUser()
    {
        var userId = this.GetUserId();

        var user = await _userService.GetByIdAsync(userId);

        return Ok(user);
    }

    [HttpGet("{id}")]
    [Authorize(Roles = "Admin")]
    public async Task<ActionResult<UserDTO>> GetUser(Guid id)
    {
        var user = await _userService.GetByIdAsync(id);
        if (user == null)
            return NotFound();

        return Ok(user);
    }

    [HttpPost]
    [Authorize(Roles = "Admin")]
    public async Task<ActionResult<UserDTO>> CreateUser(CreateUserDTO dto)
    {
        var user = await _userService.CreateAsync(dto);
        return CreatedAtAction(nameof(GetUser), new { id = user.Id }, user);
    }

    [HttpPut("{id}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> UpdateUserByAdmin(Guid id, UpdateUserDTO dto)
    {
        var success = await _userService.UpdateAsync(id, dto);

        if (!success)
            return NotFound();

        return NoContent();
    }

    [HttpPut("me")]
    public async Task<IActionResult> UpdateOwnProfile(UpdateSelfUserDTO dto)
    {
        var userId = this.GetUserId();

        var fullDto = new UpdateUserDTO
        {
            Name = dto.Name,
            Password = dto.Password,
            IsAdmin = null,
        };

        var success = await _userService.UpdateAsync(userId, fullDto);

        if (!success)
            return NotFound();

        return NoContent();
    }

    [HttpDelete("{id}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> DeleteUser(Guid id)
    {
        var success = await _userService.DeleteAsync(id);
        if (!success)
            return NotFound();

        return NoContent();
    }
}