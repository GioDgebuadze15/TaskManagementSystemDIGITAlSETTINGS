using Microsoft.AspNetCore.Http;

namespace TMS.Data.Forms;

public class CreateTaskForm
{
    public string Title { get; set; }
    public string? ShortDescription { get; set; }
    public string? Description { get; set; }

    public IFormFile? File { get; set; }

    public string Username { get; set; }

}