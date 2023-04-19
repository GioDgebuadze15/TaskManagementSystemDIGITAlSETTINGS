using System.Security.Claims;
using Microsoft.AspNetCore.Identity;
using TMS.Data.Forms;
using TMS.Data.Models;

namespace TMS.Services.AppServices.AdminAppService;

public class AdminService : IAdminService
{
    private readonly UserManager<IdentityUser> _userManager;

    public AdminService(UserManager<IdentityUser> userManager)
    {
        _userManager = userManager;
    }


    public async Task<SignUpResponse> CreateUser(CreateUserForm createUserForm)
    {
        var user = new IdentityUser
        {
            UserName = createUserForm.Username
        };
        var createUserResult = await _userManager.CreateAsync(user, createUserForm.Password);

        return !createUserResult.Succeeded
            ? new SignUpResponse(400, "Username already exists!")
            : new SignUpResponse(200, null);
    }

    public async Task<AssignRolesResponse> AssignRolesToUser(AssignRolesForm assignRolesForm)
    {
        var user = await _userManager.FindByNameAsync(assignRolesForm.Username);
        if (user is null) return new AssignRolesResponse(404, "User with this username doesn't exist!", null);

        var userClaims = await GetClaimsForUser(user);

        foreach (var roleName in from role in assignRolesForm.Roles
                 select Enum.GetName(typeof(UserRole), role)
                 into roleName
                 where !userClaims.Any(c => c.Type == "role" && c.Value == roleName)
                 where roleName != null
                 select roleName)
        {
            await _userManager.AddClaimAsync(user, new Claim("role", roleName));
        }

        var claims = await GetClaimsForUser(user);
        return new AssignRolesResponse(200, null, claims?.Select(x => x.Value));
    }

    private async Task<IList<Claim>?> GetClaimsForUser(IdentityUser user)
        => await _userManager.GetClaimsAsync(user);
}