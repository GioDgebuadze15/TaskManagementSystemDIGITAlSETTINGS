using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using TMS.Data.Forms;
using TMS.Data.Models;

namespace TMS.Services.AppServices.UserAppService;

public class UserService : IUserService
{
    private readonly UserManager<IdentityUser> _userManager;
    private readonly SignInManager<IdentityUser> _signInManager;

    public UserService(UserManager<IdentityUser> userManager, SignInManager<IdentityUser> signInManager)
    {
        _userManager = userManager;
        _signInManager = signInManager;
    }

    public async Task<SignInResponse> LoginUser(LoginUserForm loginUserForm)
    {
        var result = await _signInManager.PasswordSignInAsync(
            loginUserForm.Username,
            loginUserForm.Password,
            true,
            false);
        if (!result.Succeeded)
            return new SignInResponse(404, "Incorrect data!", null);

        var user = await _userManager.FindByNameAsync(loginUserForm.Username);
        var userClaims = await _userManager.GetClaimsAsync(user);
        var token = JwtTokenHelper.GenerateJwtToken(user, userClaims);
        return new SignInResponse(200, null, token);
    }

    public async Task<IEnumerable<string>> GetAllUserNames()
        => await _userManager.Users.Select(x => x.UserName).ToListAsync();
}