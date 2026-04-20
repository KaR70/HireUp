namespace HireUp.Errors;

public static class LocationErrors
{
    public static readonly Error NotFound = 
        new ("Location.NotFound", "Location not Found", StatusCodes.Status404NotFound);
}