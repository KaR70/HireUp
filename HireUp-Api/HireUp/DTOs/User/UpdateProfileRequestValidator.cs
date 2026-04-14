using FluentValidation;

namespace HireUp.DTOs.User;

public class UpdateProfileRequestValidator : AbstractValidator<UpdateProfileRequest>
{
    public UpdateProfileRequestValidator()
    {
        RuleFor(x => x.FirstName)
            .NotEmpty()
            .Length(2, 50);

        RuleFor(x => x.LastName)
            .NotEmpty()
            .Length(2, 50);

        // TODO: Uncomment this
        // RuleFor(x => x.JobRoleId)
        //     .GreaterThan(0)
        //     .NotEmpty();
        
        RuleFor(x => x.Header)
            .Length(2, 50)
            .When(x => !string.IsNullOrEmpty(x.Header));
        
        RuleFor(x => x.Gender)
            .Must((x) => x.ToLowerInvariant() is "male" or "female")
            .WithMessage("Invalid gender specified")
            .When(x => !string.IsNullOrEmpty(x.Gender));
        
        RuleFor(x => x.PhoneNumber)
            .MaximumLength(20)
            .WithMessage("Phone number is too long.")
            .Matches(@"^[\+]?[(]?[0-9]{3}[)]?[-\s\.]?[0-9]{3}[-\s\.]?[0-9]{4,6}$")
            .WithMessage("Invalid phone number format.")
            .When(x => !string.IsNullOrEmpty(x.PhoneNumber));
        
        RuleFor(x => x.Birthday)
            .LessThan(DateOnly.FromDateTime(DateTime.UtcNow))
            .WithMessage("Birthday must be a date in the past")
            .When(x => x.Birthday.HasValue);
        
        RuleFor(x => x.Country)
            .MaximumLength(100).WithMessage("Country name is too long.")
            .When(x => !string.IsNullOrEmpty(x.Country));

        RuleFor(x => x.City)
            .MaximumLength(100).WithMessage("City name is too long.")
            .When(x => !string.IsNullOrEmpty(x.City));
    }
}
