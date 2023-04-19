using TMS.Data.Forms;
using TMS.Data.Models;

namespace TMS.Services.AppServices.TaskAppService;

public interface ITaskService
{
    object GetTaskById(int id);
    object GetTaskByUsername(string username);
    IEnumerable<object> GetAllTasks();
    Task<CreateTaskResponse> CreateTask(CreateTaskForm createTaskForm);
    Task<UpdateTaskResponse> UpdateTask(UpdateTaskForm updateTaskForm);
    Task<DeleteTaskResponse> DeleteTask(int id);
}