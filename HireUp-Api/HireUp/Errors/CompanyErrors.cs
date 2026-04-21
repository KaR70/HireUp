namespace HireUp.Errors;

public static class CompanyErrors
{
    public static readonly Error CreationFailed = 
        new("Company.CreationFailed", "Could not create the company profile.", StatusCodes.Status500InternalServerError);
    
    public static readonly Error NotFound = 
        new("Company.NotFound", "The requested company profile was not found", StatusCodes.Status404NotFound);
}