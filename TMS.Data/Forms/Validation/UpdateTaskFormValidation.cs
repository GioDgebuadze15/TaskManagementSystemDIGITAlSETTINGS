using FluentValidation;

namespace TMS.Data.Forms.Validation;

public class UpdateTaskFormValidation : AbstractValidator<UpdateTaskForm>
{
    public UpdateTaskFormValidation()
    {
        RuleFor(task => task.Id)
            .NotEqual(0);

        RuleFor(task => task.Title)
            .NotNull()
            .WithMessage("Title is required.");

        RuleFor(task => task.Username)
            .NotNull()
            .WithMessage("Username is required.");
        
    }
}