using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;

namespace TMS.Data.Models;

public class TaskEntity : BaseModel<int>
{
    [Required] public string Title { get; set; }
    public string? ShortDescription { get; set; }
    public string? Description { get; set; }

    public File? File { get; set; }
    public string UserId { get; set; }
    public IdentityUser User { get; set; }
}