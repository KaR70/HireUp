namespace HireUp.Errors;

public static class AccessbilityNeedsErrors
{
    public static readonly Error NotFound = 
        new ("AccessbilityNeeds.NotFound", "AccessbilityNeeds not Found", StatusCodes.Status404NotFound);
}