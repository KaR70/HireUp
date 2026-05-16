namespace HireUp.Errors;

public static class DisabilityTypesErrors
{
    public static readonly Error NotFound = 
        new ("DisabilityTypes.NotFound", "DisabilityTypes not Found", StatusCodes.Status404NotFound);
}