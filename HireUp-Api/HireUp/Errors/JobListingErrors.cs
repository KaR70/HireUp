namespace HireUp.Errors;

public static class JobListingErrors
{
    public static readonly Error NotFound = 
        new ("JobListing.NotFound", "Job Wasn't Found", StatusCodes.Status404NotFound);
}