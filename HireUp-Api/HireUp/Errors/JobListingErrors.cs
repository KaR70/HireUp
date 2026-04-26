namespace HireUp.Errors;

public static class JobListingErrors
{
    public static readonly Error NotFound = 
        new ("JobListing.NotFound", "Job Wasn't Found", StatusCodes.Status404NotFound);
    public static readonly Error Forbidden = 
        new ("JobListing.Forbidden", "You do not have permission to modify or delete this job listing.", StatusCodes.Status403Forbidden);
}