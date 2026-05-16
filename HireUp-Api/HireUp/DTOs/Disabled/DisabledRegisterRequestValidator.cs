namespace HireUp.DTOs.Disabled;

public class DisabledRegisterRequestValidator : AbstractValidator<DisabledRegisterRequest>
{
    public DisabledRegisterRequestValidator()
    {
        RuleFor(x => x.FirstName)
            .NotEmpty()
            .Length(2, 50);
        
        RuleFor(x => x.LastName)
            .NotEmpty()
            .Length(2, 50);

        RuleFor(x => x.Email)
            .NotEmpty()
            .EmailAddress();

        RuleFor(x => x.LocationId)
            .NotEmpty()
            .GreaterThan(0);
        
        RuleFor(x => x.Gender)
            .Must((x) => x.ToLowerInvariant() is "male" or "female")
            .WithMessage("Invalid gender specified");
        
        RuleFor(x => x.PhoneNumber)
            .MaximumLength(20)
            .WithMessage("Phone number is too long.")
            .Matches(@"^[\+]?[(]?[0-9]{3}[)]?[-\s\.]?[0-9]{3}[-\s\.]?[0-9]{4,6}$")
            .WithMessage("Invalid phone number format.");
        
        RuleFor(x => x.Birthday)
            .LessThan(DateOnly.FromDateTime(DateTime.UtcNow))
            .WithMessage("Birthday must be a date in the past");
        
        RuleFor(x => x.Password)
            .NotEmpty()
            .Matches(RegexPatterns.Password)
            .WithMessage("Password should be at least 8 digits and should contains lowercase, NonAlphanumeric and UpperCase");
    }
}