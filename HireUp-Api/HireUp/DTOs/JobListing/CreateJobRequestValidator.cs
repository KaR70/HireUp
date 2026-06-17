namespace HireUp.DTOs.JobListing;

public class CreateJobRequestValidator : AbstractValidator<CreateJobRequest>
{
    public CreateJobRequestValidator()
    {
        RuleFor(x => x.Title)
            .NotEmpty()
            .MaximumLength(200);

        RuleFor(x => x.Description)
            .NotEmpty()
            .MinimumLength(50);

        RuleFor(x => x.Salary)
            .GreaterThan(0);

        RuleFor(x => x.JobTypeId)
            .GreaterThan(0)
            .WithMessage("Please select a valid job type.");

        RuleFor(x => x.ExperienceLevelId)
            .GreaterThan(0)
            .WithMessage("Please select an experience level.");

        RuleForEach(x => x.AccessibilityNeedIds)
            .GreaterThan(0)
            .WithMessage("Invalid Accessibility ID selected.");
    }
}