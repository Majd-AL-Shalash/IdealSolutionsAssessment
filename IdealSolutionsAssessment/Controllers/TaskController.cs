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

    [HttpGet]
    [Authorize(Roles = "Admin")]
    public async Task<ActionResult<IEnumerable<TaskDTO>>> GetTasks()
    {
        var tasks = await _taskService.GetAllAsync();

        return Ok(tasks);
    }

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

    [HttpPost]
    [Authorize(Roles = "Admin")]
    public async Task<ActionResult<TaskDTO>> CreateTask(CreateTaskDTO dto)
    {
        var task = await _taskService.CreateAsync(dto);
        return CreatedAtAction(nameof(GetTask), new { id = task.Id }, task);
    }

    [HttpPut("{id}")]
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

    [HttpDelete("{id}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> DeleteTask(Guid id)
    {
        var success = await _taskService.DeleteAsync(id);
        if (!success)
            return NotFound();

        return NoContent();
    }
}
