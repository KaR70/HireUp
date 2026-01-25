namespace HireUp.Services;

public interface IFileService
{
    Task<Result<string>> SaveFileAsync(IFormFile file, string folderName, CancellationToken cancellationToken = default);
    Result DeleteFile(string folderName, string fileName);
}