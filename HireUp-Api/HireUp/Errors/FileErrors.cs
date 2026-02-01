namespace HireUp.Errors;

public static class FileErrors
{
    public static readonly Error ProfilePictureUploadFailed =
        new ("File.ProfilePictureUploadFailed", "Failed to upload profile picture.", StatusCodes.Status500InternalServerError);
    
    public static readonly Error InvalidProfilePicture =
        new ("File.InvalidProfilePicture", "The profile picture is missing or empty.", StatusCodes.Status400BadRequest);
}