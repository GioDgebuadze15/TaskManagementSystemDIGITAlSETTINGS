using FluentValidation;
using TMS.Data.Models;

namespace TMS.Data.Forms.Validation;

public class AssignRolesFormValidation : AbstractValidator<AssignRolesForm>
{
    public AssignRolesFormValidation()
    {
        RuleFor(user => user.Username)
            .NotNull()
            .WithMessage("Username is required.");

        RuleFor(user => user.Roles)
            .NotEmpty()
            .WithMessage("At least one role must be provided.");

        RuleFor(user => user.Roles)
            .Must(roleList => roleList.All(role => Enum.IsDefined(typeof(UserRole), role)))
            .WithMessage("Incorrect role.");
    }
}