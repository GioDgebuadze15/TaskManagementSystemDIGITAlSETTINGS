using FluentValidation;

namespace TMS.Data.Forms.Validation;

public class LoginUserFormValidation: AbstractValidator<LoginUserForm>
{
    public LoginUserFormValidation()
    {
        RuleFor(user => user.Username)
            .NotNull()
            .WithMessage("Username is required.");
        
        RuleFor(user => user.Password)
            .NotEmpty()
            .WithMessage("Password is required.");
        
    }
}