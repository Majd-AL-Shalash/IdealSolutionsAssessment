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

    /// <summary>
    /// Gets all users. Only accessible by Admins.
    /// </summary>
    /// <returns code="200">List of all users.</returns>
    [HttpGet]
    [Authorize(Roles = "Admin")]
    public async Task<ActionResult<IEnumerable<UserDTO>>> GetUsers()
    {
        var users = await _userService.GetAllAsync();
        return Ok(users);
    }

    /// <summary>
    /// Gets the currently authenticated user's profile.
    /// </summary>
    /// <returns code="200">The current user's profile information.</returns>
    [HttpGet("me")]
    public async Task<ActionResult<UserDTO>> GetCurrentUser()
    {
        var userId = this.GetUserId();

        var user = await _userService.GetByIdAsync(userId);

        return Ok(user);
    }

    /// <summary>
    /// Gets a user by their ID. Only accessible by Admins.
    /// </summary>
    /// <param name="id">The ID of the user.</param>
    /// <response code="200">Returns the task details.</response>
    /// <response code="404">User not found.</response>    
    [HttpGet("{id}")]
    [Authorize(Roles = "Admin")]
    public async Task<ActionResult<UserDTO>> GetUser(Guid id)
    {
        var user = await _userService.GetByIdAsync(id);
        if (user == null)
            return NotFound();

        return Ok(user);
    }

    /// <summary>
    /// Creates a new user. Only accessible by Admins.
    /// </summary>
    /// <param name="dto">User creation data.</param>
    /// <returns code="200">Created user information.</returns>
    [HttpPost]
    [Authorize(Roles = "Admin")]
    public async Task<ActionResult<UserDTO>> CreateUser(CreateUserDTO dto)
    {
        var user = await _userService.CreateAsync(dto);
        return CreatedAtAction(nameof(GetUser), new { id = user.Id }, user);
    }


    /// <summary>
    /// Updates another user's profile. Only accessible by Admins.
    /// </summary>
    /// <param name="id">User ID to update.</param>
    /// <param name="dto">Updated user information.</param>
    /// <response code="204">Update succeeded.</response>
    /// <response code="404">Task not found.</response>
    [HttpPut("{id}")]
    [Authorize(Roles = "Admin")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> UpdateUserByAdmin(Guid id, UpdateUserDTO dto)
    {
        var success = await _userService.UpdateAsync(id, dto);

        if (!success)
            return NotFound();

        return NoContent();
    }

    /// <summary>
    /// Updates the currently authenticated user's profile.
    /// </summary>
    /// <param name="dto">Updated user information.</param>
    /// <response code="204">Update succeeded.</response>
    /// <response code="404">Task not found.</response>
    [HttpPut("me")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
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

    /// <summary>
    /// Deletes a user. Only accessible by Admins. Tasks with this user will also be deleted.
    /// </summary>
    /// <param name="id">The ID of the user to delete.</param>
    /// <response code="204">Delete succeeded.</response>
    /// <response code="404">User not found.</response>
    [HttpDelete("{id}")]
    [Authorize(Roles = "Admin")]
    [ProducesResponseType(StatusCodes.Status204NoContent)] 
    [ProducesResponseType(StatusCodes.Status404NotFound)]  
    public async Task<IActionResult> DeleteUser(Guid id)
    {
        var success = await _userService.DeleteAsync(id);
        if (!success)
            return NotFound();

        return NoContent();
    }
}