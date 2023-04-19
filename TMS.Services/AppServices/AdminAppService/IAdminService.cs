using TMS.Data.Forms;
using TMS.Data.Models;

namespace TMS.Services.AppServices.AdminAppService;

public interface IAdminService
{
    Task<SignUpResponse> CreateUser(CreateUserForm createUserForm);
    Task<AssignRolesResponse> AssignRolesToUser(AssignRolesForm assignRolesForm);
}