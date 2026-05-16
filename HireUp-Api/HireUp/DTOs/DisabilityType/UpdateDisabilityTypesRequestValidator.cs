namespace HireUp.DTOs.DisabilityType;

public class UpdateDisabilityTypesRequestValidator : AbstractValidator<UpdateDisabilityTypesRequest>
{
    public UpdateDisabilityTypesRequestValidator()
    {
        RuleForEach(x => x.DisabilityTypesIds)
            .GreaterThan(0);
    }
}