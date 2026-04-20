namespace HireUp.DTOs.Company;

public class RegisterProfileRequestValidator : AbstractValidator<RegisterProfileRequest>
{
    public RegisterProfileRequestValidator()
    {
        RuleFor(x => x.Email)
            .NotEmpty()
            .EmailAddress();

        RuleFor(x => x.Password)
            .NotEmpty()
            .Matches(RegexPatterns.Password)
            .WithMessage("Password should be at least 8 digits and should contains lowercase, NonAlphanumeric and UpperCase");

        RuleFor(x => x.Name)
            .NotEmpty()
            .Length(2, 100);

        RuleFor(x => x.IndustryId)
            .GreaterThan(0)
            .When(x => x.IndustryId != null)
            .WithMessage("Invalid Industry");

        RuleFor(x => x.LocationId)
            .GreaterThan(0)
            .When(x => x.LocationId != null)
            .WithMessage("Invalid Location");

        RuleFor(x => x.Description)
            .MaximumLength(100);
        
        RuleFor(x => x.FoundedYear)
            .InclusiveBetween(1800, DateTime.UtcNow.Year)
            .When(x => x.FoundedYear != null)
            .WithMessage("Founded year must be valid.");

        RuleFor(x => x.Website)
            .Matches(RegexPatterns.WebsiteUrl)
            .When(x => !string.IsNullOrEmpty(x.Website))
            .WithMessage("Please enter a valid website URL");

        RuleFor(x => x.LinkedIn)
            .Matches(RegexPatterns.LinkedInUrl)
            .When(x => !string.IsNullOrEmpty(x.LinkedIn))
            .WithMessage("Please enter a valid LinkedIn URL (e.g., https://linkedin.com/company/hireup)");

    }
}