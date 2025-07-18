using FluentValidation;
using Touhou1CCTracker.Application.DTOs.Difficulty;

namespace Touhou1CCTracker.Application.Validators;

public class DifficultyValidator : AbstractValidator<DifficultyCreateOrUpdateDto>
{
    public DifficultyValidator()
    {
        RuleFor(d => d.DifficultyName)
            .NotEmpty().WithMessage("Difficulty name is required!")
            .Length(2, 20).WithMessage("Difficulty name must be between 2 and 20 characters!");
    }
}