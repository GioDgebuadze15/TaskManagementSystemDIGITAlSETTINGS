using System.Linq.Expressions;
using TMS.Data.Models;

namespace TMS.Data.ViewModels;

public static class TaskViewModels
{
    public static Expression<Func<TaskEntity, object>> Default =>
        task => new
        {
            task.Title,
            task.ShortDescription,
            task.Description,
            task.UserId
        };
}