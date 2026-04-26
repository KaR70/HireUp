namespace HireUp.DTOs.JobListing;

public class UpdateRequestValidator : AbstractValidator<UpdateRequest>
{
    public UpdateRequestValidator()
    {
        RuleFor(x => x.Title)
            .NotEmpty()
            .MaximumLength(200);

        RuleFor(x => x.Description)
            .NotEmpty()
            .MinimumLength(20);

        RuleFor(x => x.Requirements)
            .NotEmpty();
        
        RuleFor(x => x.Salary)
            .GreaterThan(0)
            .When(x => x.Salary != null);

        RuleFor(x => x.ExpiryDate)
            .GreaterThan(DateOnly.FromDateTime(DateTime.Now))
            .WithMessage("Expiry date must be in the future.");

        RuleFor(x => x.ExperienceLevelId)
            .GreaterThan(0);
        
        RuleFor(x => x.JobRoleId)
            .GreaterThan(0);

        RuleFor(x => x.LocationId)
            .GreaterThan(0);
        
        When(x => x.IsInclusiveHiring, () =>
        {
            RuleFor(x => x.DisabilitySupport)
                .NotEmpty().WithMessage("Please describe the disability support provided since inclusive hiring is enabled.")
                .MaximumLength(500).WithMessage("Disability support description is too long.");
        });

    }
}