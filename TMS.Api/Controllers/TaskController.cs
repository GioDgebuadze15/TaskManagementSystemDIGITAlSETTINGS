using Microsoft.AspNetCore.Mvc;
using TMS.Data.Forms;
using TMS.Services.AppServices.TaskAppService;

namespace TMS.Api.Controllers;

[Route("api/task")]
public class TaskController : ApiController
{
    private readonly ITaskService _iTaskService;

    public TaskController(ITaskService iTaskService)
    {
        _iTaskService = iTaskService;
    }

    [HttpGet("{id::int}")]
    public IActionResult GetOne(int id)
        => Ok(_iTaskService.GetTaskById(id));

    [HttpGet]
    public IActionResult GetAll()
        => Ok(_iTaskService.GetAllTasks());

    [HttpPost]
    public async Task<IActionResult> Add([FromBody] CreateTaskForm createTaskForm)
    {
        var result = await _iTaskService.CreateTask(createTaskForm);

        return result.StatusCode switch
        {
            200 => Ok(result),
            _ => BadRequest(result)
        };
    }

    [HttpPut]
    public async Task<IActionResult> Update([FromBody] UpdateTaskForm updateTaskForm)
    {
        var result = await _iTaskService.UpdateTask(updateTaskForm);
        return result.StatusCode switch
        {
            400 => BadRequest(result),
            404 => NotFound(result),
            _ => Ok(result)
        };
    }
    
    [HttpDelete("{id::int}")]
    public async Task<IActionResult> Delete(int id)
    {
        var result = await _iTaskService.DeleteTask(id);
        if (result.StatusCode is 404) return NotFound(result);
        return Ok(result);
    }
}