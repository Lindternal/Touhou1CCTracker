using FluentValidation;
using Touhou1CCTracker.Application.DTOs.Login;

namespace Touhou1CCTracker.Application.Validators;

public class LoginValidator : AbstractValidator<LoginRequestDto>
{
    public LoginValidator()
    {
        RuleFor(u => u.Username)
            .NotEmpty().WithMessage("Invalid username or password!")
            .Length(3, 30).WithMessage("Invalid username or password!");

        RuleFor(u => u.Password)
            .NotEmpty().WithMessage("Invalid username or password!");
    }
}