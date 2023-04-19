using TMS.Data.Models;

namespace TMS.Data.Forms;

public class AssignRolesForm
{
    public string Username { get; set; }
    public List<UserRole> Roles { get; set; } = new();
}

