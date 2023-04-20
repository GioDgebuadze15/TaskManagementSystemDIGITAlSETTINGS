using System.Linq.Expressions;
using TMS.Data.Models;

namespace TMS.Data.ViewModels;

public static class TaskViewModels
{
    public static Expression<Func<TaskEntity, object>> Default =>
        task => new
        {
            task.Id,
            task.Title,
            task.ShortDescription,
            task.Description,
            FileName = task.File != null ? task.File.FileName : string.Empty,
            Username = task.User.UserName,
            task.UserId
        };
}