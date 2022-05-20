namespace SkillTracker.Services.Profile.API.Application.Commands;
public class AddPorfileCommandValidator : AbstractValidator<AddProfileCommand>
{
    public AddPorfileCommandValidator()
    {
        RuleFor(p => p.Name)
        .NotEmpty().WithMessage("{Name} is required.")
        .NotNull()
        .MaximumLength(50).WithMessage("{Name} must not exceed 50 characters.");

        RuleFor(p => p.Email)
           .NotEmpty().WithMessage("{Email} is required.");

        RuleFor(p => p.Skills.Count(s => s.IsTechnical))
            .GreaterThanOrEqualTo(3).WithMessage("Minimum 3 Technical sklls are required.");

        RuleFor(p => p.Skills.Count(s => !s.IsTechnical))
            .GreaterThanOrEqualTo(2).WithMessage("Minimum 2 Non-technical sklls are required.");
    }
}
