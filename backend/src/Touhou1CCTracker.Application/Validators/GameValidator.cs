using FluentValidation;
using Touhou1CCTracker.Application.DTOs.Game;

namespace Touhou1CCTracker.Application.Validators;

public class GameValidator : AbstractValidator<GameCreateOrUpdateDto>
{
    public GameValidator()
    {
        RuleFor(g => g.GameName)
            .NotEmpty().WithMessage("Game name is required!")
            .Length(2, 50).WithMessage("Game name must be between 2 and 50 characters!");
    }
}