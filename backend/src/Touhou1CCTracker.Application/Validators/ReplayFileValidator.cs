using FluentValidation;
using Touhou1CCTracker.Application.DTOs.ReplayFile;

namespace Touhou1CCTracker.Application.Validators;

public class ReplayFileValidator : AbstractValidator<ReplayFileUploadDto>
{
    public ReplayFileValidator()
    {
        RuleFor(f => f.RecordId)
            .NotEmpty().WithMessage("Id must be provided!");
        
        RuleFor(f => f.File)
            .Custom((file, context) =>
            {
                if (file == null) return;
                if (file.Length > 1024 * 1024)
                    context.AddFailure("File is too large! 1Mb max!");
                if (Path.GetExtension(file.FileName).ToLower() != ".rpy")
                    context.AddFailure("Wrong file extension!");
            });
    }
}