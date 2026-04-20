namespace HireUp.Errors;

public static class CompanyErrors
{
    public static readonly Error CreationFailed = 
        new("Company.CreationFailed", "Could not create the company profile.", StatusCodes.Status500InternalServerError);
}