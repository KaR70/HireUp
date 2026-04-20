namespace HireUp.Errors;

public static class IndustryErrors
{
    public static readonly Error NotFound = 
        new ("Industry.NotFound", "Industry not Found", StatusCodes.Status404NotFound);
}