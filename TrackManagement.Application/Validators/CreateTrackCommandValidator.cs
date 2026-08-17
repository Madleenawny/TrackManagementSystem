using FluentValidation;
using TrackManagement.Application.DTOs;

namespace TrackManagement.Application.Validators;

public class CreateTrackCommandValidator : AbstractValidator<CreateTrackDto>
{
      public CreateTrackCommandValidator()
      {
            RuleFor(x => x.Title)
                .NotEmpty().WithMessage("العنوان مطلوب")
                .MaximumLength(200).WithMessage("العنوان لا يتجاوز 200 حرف");

            RuleFor(x => x.Isrc)
                .NotEmpty().WithMessage("كود ISRC مطلوب")
                .Matches(@"^[A-Z]{2}-[A-Z0-9]{3}-\d{2}-\d{5}$|^[A-Z0-9]{12}$")
                .WithMessage("صيغة كود ISRC غير صحيحة");

            RuleFor(x => x.ArtistId)
                .GreaterThan(0).WithMessage("يجب اختيار فنان صحيح");
      }
}