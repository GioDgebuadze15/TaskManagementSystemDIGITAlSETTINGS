using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using TMS.Data.Forms;
using TMS.Data.Models;
using TMS.Data.ViewModels;
using TMS.Database.DatabaseRepository;
using File = TMS.Data.Models.File;

namespace TMS.Services.AppServices.TaskAppService;

public class TaskService : ITaskService
{
    private readonly IRepository<TaskEntity> _ctx;
    private readonly UserManager<IdentityUser> _userManager;
    private readonly ILogger<TaskService> _logger;
    private readonly IWebHostEnvironment _env;

    public TaskService(IRepository<TaskEntity> ctx, UserManager<IdentityUser> userManager, ILogger<TaskService> logger,
        IWebHostEnvironment env)
    {
        _ctx = ctx;
        _userManager = userManager;
        _logger = logger;
        _env = env;
    }

    public object GetTaskById(int id)
    {
        try
        {
            return _ctx.GetById(id)
                .Include(x => x.File)
                .Include(x => x.User)
                .Select(TaskViewModels.Default.Compile())
                .FirstOrDefault() ?? new object();
        }
        catch (InvalidOperationException ex)
        {
            _logger.LogError(ex, ex.Message);
            return new object();
        }
    }

    public object GetTaskByUsername(string username)
        => _ctx.GetAll()
            .Include(x => x.File)
            .Include(x => x.User)
            .Where(x => x.User.UserName.Equals(username)).AsEnumerable()
            .Select(TaskViewModels.Default.Compile())
            .FirstOrDefault() ?? new object();


    public IEnumerable<object> GetAllTasks()
        => _ctx.GetAll()
            .Include(x => x.File)
            .Include(x => x.User)
            .Select(TaskViewModels.Default.Compile());

    public async Task<CreateTaskResponse> CreateTask(CreateTaskForm createTaskForm)
    {
        var user = await _userManager.FindByNameAsync(createTaskForm.Username);
        if (user is null) return new CreateTaskResponse(400, "User doesn't exists!", null);

        string? fileName = null;
        if (createTaskForm.File is not null)
            fileName = await SaveFile(createTaskForm.File);
        var task = new TaskEntity
        {
            Title = createTaskForm.Title,
            ShortDescription = createTaskForm.ShortDescription,
            Description = createTaskForm.Description,
            UserId = user.Id,
            File = fileName is not null ? new File {FileName = fileName} : null
        };

        var result = await _ctx.Add(task);
        return new CreateTaskResponse(200, null, TaskViewModels.Default.Compile().Invoke(result));
    }

    public async Task<UpdateTaskResponse> UpdateTask(UpdateTaskForm updateTaskForm)
    {
        var user = await _userManager.FindByNameAsync(updateTaskForm.Username);
        if (user is null) return new UpdateTaskResponse(400, "User doesn't exists!", null);

        try
        {
            var task = _ctx.GetById(updateTaskForm.Id)
                .Include(x => x.File)
                .Include(x => x.User)
                .FirstOrDefault();

            task!.Title = updateTaskForm.Title;
            task.ShortDescription = updateTaskForm.ShortDescription;
            task.Description = updateTaskForm.Description;
            task.UserId = user.Id;

            if (updateTaskForm.File is not null)
            {
                if (task.File?.FileName is { })
                    DeleteFile(task.File.FileName);

                var fileName = await SaveFile(updateTaskForm.File);
                task.File = new File {FileName = fileName};
            }


            await _ctx.Update(task);
            return new UpdateTaskResponse(200, null, TaskViewModels.Default.Compile().Invoke(task));
        }
        catch (InvalidOperationException ex)
        {
            _logger.LogError(ex, ex.Message);
            return new UpdateTaskResponse(404, "Cant find task to edit.", null);
        }
    }

    public async Task<DeleteTaskResponse> DeleteTask(int id)
    {
        try
        {
            var task = _ctx.GetById(id)
                .Include(x => x.File)
                .Include(x => x.User)
                .FirstOrDefault();

            await _ctx.Delete(task ?? throw new InvalidOperationException("Cant find task to delete."));

            if (task.File?.FileName is { })
                DeleteFile(task.File.FileName);

            return new DeleteTaskResponse(200, null, TaskViewModels.Default.Compile().Invoke(task));
        }
        catch (InvalidOperationException ex)
        {
            _logger.LogError(ex, ex.Message);
            return new DeleteTaskResponse(404, "Cant find task to delete.", null);
        }
    }


    private async Task<string> SaveFile(IFormFile file)
    {
        var mime = file.FileName.Split('.').Last();
        var fileName = string.Concat(Path.GetRandomFileName(), ".", mime);
        var savePath = Path.Combine(_env.WebRootPath, "files", fileName);

        await using var fileStream = new FileStream(savePath, FileMode.Create, FileAccess.Write);
        await file.CopyToAsync(fileStream);

        return fileName;
    }

    private string GetSavePath(string image)
        =>
            Path.Combine(_env.WebRootPath, "files", image);

    private void DeleteFile(string fileFileName)
        =>
            System.IO.File.Delete(GetSavePath(fileFileName));
}