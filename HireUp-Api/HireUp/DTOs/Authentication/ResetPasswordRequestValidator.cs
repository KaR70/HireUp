namespace HireUp.DTOs.Authentication;

public class ResetPasswordRequestValidator: AbstractValidator<ResetPasswordRequest>
{
    public ResetPasswordRequestValidator()
    {
        RuleFor(x => x.Email)
            .NotEmpty()
            .EmailAddress();

        RuleFor(x => x.Code)
            .NotEmpty().WithMessage("Code is required.")
            .Length(5).WithMessage("Code must be 5 digits.")
            .Matches("^[0-9]*$").WithMessage("Code must only contain numbers.");

        RuleFor(x => x.NewPassword)
            .Matches(RegexPatterns.Password)
            .WithMessage("Password should be at least 8 digits and should contains lowercase, NonAlphanumeric and UpperCase");
    }
}