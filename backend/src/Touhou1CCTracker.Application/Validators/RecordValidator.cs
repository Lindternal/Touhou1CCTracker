using FluentValidation;
using Touhou1CCTracker.Application.DTOs.Record;

namespace Touhou1CCTracker.Application.Validators;

public class RecordValidator : AbstractValidator<RecordCreateOrUpdateDto>
{
    public RecordValidator()
    {
        RuleFor(r => r.Rank)
            .NotEmpty().WithMessage("Please specify a rank!")
            .Length(1, 70).WithMessage("Must be between 1 and 70!");
        
        RuleFor(r => r.GameId)
            .NotEmpty().WithMessage("Game is required!");
        
        RuleFor(r => r.DifficultyId)
            .NotEmpty().WithMessage("Difficulty is required!");
        
        RuleFor(r => r.ShotTypeId)
            .NotEmpty().WithMessage("Shot type is required!!");

        RuleFor(r => r.Date)
            .LessThanOrEqualTo(DateOnly.FromDateTime(DateTime.UtcNow));
        
        RuleFor(r => r.VideoUrl)
            .Length(0, 100).WithMessage("Video url length must be less than 100!");
    }
}