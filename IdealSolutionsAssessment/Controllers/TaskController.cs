using IdealSolutionsAssessment.DTOs;
using IdealSolutionsAssessment.Enums;
using IdealSolutionsAssessment.Helpers;
using IdealSolutionsAssessment.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace IdealSolutionsAssessment.Controllers;

[Route("api/[controller]")]
[ApiController]
[Authorize]
public class TaskController : ControllerBase
{
    private readonly ITaskService _taskService;

    public TaskController(ITaskService taskService)
    {
        _taskService = taskService;
    }

    /// <summary>
    /// Gets all tasks. Only accessible by Admins.
    /// </summary>
    /// <returns>List of all tasks.</returns>
    [HttpGet]
    [Authorize(Roles = "Admin")]
    public async Task<ActionResult<IEnumerable<TaskDTO>>> GetTasks()
    {
        var tasks = await _taskService.GetAllAsync();

        return Ok(tasks);
    }

    /// <summary>
    /// Gets a task by its ID. Accessible by both Admin and regular users.
    /// </summary>
    /// <param name="id">The ID of the task.</param>
    /// <returns>
    /// Returns the task details if found, otherwise returns NotFound.
    /// </returns>
    /// <response code="200">Returns the task details.</response>
    /// <response code="403">User is not authorized to access the task.</response>
    /// <response code="404">Task not found.</response>
    [HttpGet("{id}")]
    public async Task<ActionResult<TaskDTO>> GetTask(Guid id)
    {
        var userId = this.GetUserId();
        var isAdmin = this.IsAdmin();

        var (taskFlag, task) = await _taskService.GetByIdAsync(id, userId, isAdmin);

        return taskFlag switch
        {
            GetTaskFlags.Success => Ok(task),
            GetTaskFlags.Forbidden => Forbid(),
            GetTaskFlags.TaskNotFound => NotFound(),
            _ => throw new NotImplementedException(),
        };
    }

    /// <summary>
    /// Creates a new task. Only accessible by Admins.
    /// </summary>
    /// <param name="dto">Task creation data.</param>
    /// <returns>Created task information.</returns>
    [HttpPost]
    [Authorize(Roles = "Admin")]
    public async Task<ActionResult<TaskDTO>> CreateTask(CreateTaskDTO dto)
    {
        var task = await _taskService.CreateAsync(dto);
        return CreatedAtAction(nameof(GetTask), new { id = task.Id }, task);
    }

    /// <summary>
    /// Updates a task by its ID. Accessible by both Admin and regular users.
    /// </summary>
    /// <param name="id">The ID of the task to update.</param>
    /// <param name="dto">Updated task data.</param>
    /// <response code="204">Update succeeded.</response>
    /// <response code="403">User is not authorized to update the task.</response>
    /// <response code="404">Task not found.</response>
    [HttpPut("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> UpdateTask(Guid id, UpdateTaskDTO dto)
    {
        var userId = this.GetUserId();
        var isAdmin = this.IsAdmin();

        var updateResult = await _taskService.UpdateAsync(id, dto, userId, isAdmin);

        return updateResult switch
        {
            UpdateTaskFlags.Success => NoContent(),
            UpdateTaskFlags.Forbidden => Forbid(),
            UpdateTaskFlags.TaskNotFound => NotFound(),
            _ => throw new NotImplementedException()
        };
    }

    /// <summary>
    /// Deletes a task by its ID. Only accessible by Admins.
    /// </summary>
    /// <param name="id">The ID of the task to delete.</param>
    /// <response code="204">Delete succeeded.</response>
    /// <response code="404">Task not found.</response>
    [HttpDelete("{id}")]
    [Authorize(Roles = "Admin")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> DeleteTask(Guid id)
    {
        var success = await _taskService.DeleteAsync(id);
        if (!success)
            return NotFound();

        return NoContent();
    }
}
