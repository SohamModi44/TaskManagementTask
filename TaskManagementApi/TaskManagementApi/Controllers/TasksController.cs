using Microsoft.AspNetCore.Mvc;
using TaskManagementApi.Models;
using TaskManagementApi.Services;

namespace TaskManagementApi.Controllers;

[ApiController]
[Route("api/tasks")]
public class TasksController : ControllerBase
{
    private readonly ITaskService _taskService;
    private readonly ITaskUtilityService _taskUtilityService;

    public TasksController(ITaskService taskService, ITaskUtilityService taskUtilityService)
    {
        _taskService = taskService;
        _taskUtilityService = taskUtilityService;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<WorkTask>>> GetAll()
    {
        var tasks = await _taskService.GetAllTasksAsync();
        return Ok(tasks);
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<WorkTask>> GetById(int id)
    {
        var task = await _taskService.GetTaskByIdAsync(id);
        if (task == null) return NotFound(new { message = "Task not found." });
        return Ok(task);
    }

    [HttpPost]
    public async Task<ActionResult<WorkTask>> Create([FromBody] WorkTask task)
    {
        if (!Enum.IsDefined(typeof(Models.TaskStatus), task.Status))
            return BadRequest(new { message = "Invalid task status." });

        var createdTask = await _taskService.CreateTaskAsync(task);
        return CreatedAtAction(nameof(GetById), new { id = createdTask.Id }, createdTask);
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update(int id, [FromBody] WorkTask updatedTask)
    {
        if (!Enum.IsDefined(typeof(Models.TaskStatus), updatedTask.Status))
            return BadRequest(new { message = "Invalid task status." });

        var success = await _taskService.UpdateTaskAsync(id, updatedTask);
        if (!success) return NotFound(new { message = "Task not found." });

        return NoContent();
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id)
    {
        var success = await _taskService.DeleteTaskAsync(id);
        if (!success) return NotFound(new { message = "Task not found." });

        return NoContent();
    }

    [HttpPost("allocate")]
    public ActionResult<AllocateTasksResponse> AllocateWorkload([FromBody] AllocateTasksRequest request)
    {
        var result = _taskUtilityService.AllocateWorkload(request);
        return Ok(result);
    }

    [HttpPost("bulk-update-status")]
    public async Task<ActionResult<BatchUpdateResult>> BulkUpdateStatus([FromBody] BulkUpdateRequest request)
    {
        var result = await _taskUtilityService.BulkUpdateStatusAsync(request.TaskIds, request.NewStatus);
        return Ok(result);
    }
}