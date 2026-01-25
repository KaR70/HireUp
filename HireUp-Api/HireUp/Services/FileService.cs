namespace HireUp.Services;

public class FileService : IFileService
{
    private readonly IWebHostEnvironment _webHostEnvironment;
    
    public FileService(IWebHostEnvironment webHostEnvironment)
    {
        _webHostEnvironment = webHostEnvironment;
    }

    public async Task<Result<string>> SaveFileAsync(IFormFile file, string folderName, CancellationToken cancellationToken = default)
    {
        if (file is null || file.Length == 0)
            return Result.Failure<string>(FileErrors.InvalidProfilePicture);
        
        var fileExtension = Path.GetExtension(file.FileName);
        var uniqueFileName = $"{Guid.NewGuid()}{fileExtension}";
        
        var folderPath = Path.Combine(_webHostEnvironment.WebRootPath, folderName);
        var filePath = Path.Combine(folderPath, uniqueFileName);
        
        Directory.CreateDirectory(folderPath);

        using (var stream = File.Create(filePath))
        {
            await file.CopyToAsync(stream, cancellationToken);
        }
        
        var relativePath = Path.Combine(folderName, uniqueFileName).Replace('\\', '/');
        return Result.Success(relativePath);
    }

    public Result DeleteFile(string folderName, string fileName)
    {
        if (string.IsNullOrWhiteSpace(fileName))
            return Result.Success();
        
        var folderPath = Path.Combine(_webHostEnvironment.WebRootPath, folderName);
        var filePath = Path.Combine(folderPath, fileName);
        
        if (File.Exists(filePath))
            File.Delete(filePath);
        
        return Result.Success();
    }
}