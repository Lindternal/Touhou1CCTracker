using FluentValidation;
using Touhou1CCTracker.Application.DTOs.Login;

namespace Touhou1CCTracker.Application.Validators;

public class RegisterValidator : AbstractValidator<RegisterDto>
{
    public RegisterValidator()
    {
        RuleFor(u => u.Username)
            .NotEmpty().WithMessage("Username is required!")
            .Length(3, 30).WithMessage("Username must be between 3 and 30 characters!");

        RuleFor(u => u.Password)
            .NotEmpty().WithMessage("Password is required!");

        RuleFor(u => u.ConfirmPassword)
            .NotEmpty().WithMessage("Please confirm your password!")
            .Equal(u => u.Password).WithMessage("Passwords do not match!");
    }
}