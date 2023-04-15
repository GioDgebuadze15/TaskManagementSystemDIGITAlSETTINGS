using Microsoft.AspNetCore.Identity;
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

        return result.Succeeded
            ? new SignInResponse(200, null)
            : new SignInResponse(404, "Incorrect data!");
    }

    public async Task<SignUpResponse> CreateUser(CreateUserForm createUserForm)
    {
        var user = new IdentityUser
        {
            UserName = createUserForm.Username
        };
        var createUserResult = await _userManager.CreateAsync(user, createUserForm.Password);

        if (!createUserResult.Succeeded)
            return new SignUpResponse(400, "Username already exists!");

        await _signInManager.SignInAsync(user, true);
        return new SignUpResponse(200, null);
    }
}