using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TMS.Data.Forms;
using TMS.Services.AppServices.UserAppService;

namespace TMS.Api.Controllers;

[Route("api/user")]
[AllowAnonymous]
public class UserController : ApiController
{
    private readonly IUserService _iUserService;

    public UserController(IUserService iUserService)
    {
        _iUserService = iUserService;
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login(LoginUserForm loginUserForm)
    {
        var result = await _iUserService.LoginUser(loginUserForm);
        if (result.StatusCode is 404) return NotFound(result);
        return Ok(result);
    }
    
    [HttpPost("register")]
    public async Task<IActionResult> Register(CreateUserForm createUserForm)
    {
        var result = await _iUserService.CreateUser(createUserForm);
        if (result.StatusCode is 400) return BadRequest(result);
        return Ok(result);
    }
}