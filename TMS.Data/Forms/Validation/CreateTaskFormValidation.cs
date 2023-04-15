using FluentValidation;

namespace TMS.Data.Forms.Validation;

public class CreateTaskFormValidation: AbstractValidator<CreateTaskForm>
{
    public CreateTaskFormValidation()
    {
        RuleFor(task => task.Title)
            .NotNull()
            .WithMessage("Title is required.");

        RuleFor(task => task.Username)
            .NotNull()
            .WithMessage("Username is required.");
    }
}