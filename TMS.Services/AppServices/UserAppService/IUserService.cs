using TMS.Data.Forms;
using TMS.Data.Models;

namespace TMS.Services.AppServices.UserAppService;

public interface IUserService
{
    Task<SignInResponse> LoginUser(LoginUserForm loginUserForm);

    Task<SignUpResponse> CreateUser(CreateUserForm createUserForm);
}