namespace TMS.Data.Forms;

public class UpdateTaskForm : CreateTaskForm
{
    public int Id { get; set; }
    public bool Deleted { get; set; } = false;
}