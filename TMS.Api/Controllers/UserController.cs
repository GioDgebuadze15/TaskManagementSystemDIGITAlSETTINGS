using Microsoft.AspNetCore.Mvc;
using TMS.Data.Forms;
using TMS.Services.AppServices.UserAppService;

namespace TMS.Api.Controllers;

[Route("api/user")]
public class UserController : ApiController
{
    private readonly IUserService _iUserService;

    public UserController(IUserService iUserService)
    {
        _iUserService = iUserService;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
        => Ok(await _iUserService.GetAllUserNames());

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginUserForm loginUserForm)
    {
        var result = await _iUserService.LoginUser(loginUserForm);
        if (result.StatusCode is 404) return NotFound(result);
        return Ok(result);
    }
}