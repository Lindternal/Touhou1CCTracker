using FluentValidation;
using Touhou1CCTracker.Application.DTOs.ShotType;

namespace Touhou1CCTracker.Application.Validators;

public class ShotTypeValidator : AbstractValidator<ShotTypeCreateOrUpdateDto>
{
    public ShotTypeValidator()
    {
        RuleFor(s => s.CharacterName)
            .NotEmpty().WithMessage("Please specify character name!")
            .Length(1, 20).WithMessage("Character name must be between 1 and 20 characters!");

        RuleFor(s => s.ShotName)
            .Length(0, 20).WithMessage("Shot type must be less than 20 characters!");
    }
}