using System.ComponentModel.DataAnnotations;

namespace TMS.Data.Models;

public class File : BaseModel<int>
{
    [Required] public string FileName { get; set; }

    public int TaskId { get; set; }
    public TaskEntity Task { get; set; }
}