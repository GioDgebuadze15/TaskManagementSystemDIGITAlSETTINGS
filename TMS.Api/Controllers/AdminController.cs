using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TMS.Data.Forms;
using TMS.Services.AppServices.AdminAppService;

namespace TMS.Api.Controllers;

[Route("api/admin")]
[Authorize(Roles = TmsConstants.Roles.Admin)]
public class AdminController : ApiController
{
    private readonly IAdminService _iAdminService;

    public AdminController(IAdminService iAdminService)
    {
        _iAdminService = iAdminService;
    }


    [HttpPost("add-user")]
    public async Task<IActionResult> AddUser([FromBody] CreateUserForm createUserForm)
    {
        var result = await _iAdminService.CreateUser(createUserForm);
        if (result.StatusCode is 400) return BadRequest(result);
        return Ok(result);
    }

    [HttpPost("assign-roles")]
    public async Task<IActionResult> AssignRoles([FromBody] AssignRolesForm assignRolesForm)
    {
        var result = await _iAdminService.AssignRolesToUser(assignRolesForm);
        if (result.StatusCode is 404) return NotFound(result);
        return Ok(result);
    }
}