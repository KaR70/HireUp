using HireUp.Mapping;
using Microsoft.Extensions.Options;

namespace HireUp.Services;

public class UrlBuilderService
{
    private readonly string _baseUrl;

    public UrlBuilderService(IOptions<ApplicationSettings> appSettings)
    {
        _baseUrl = appSettings.Value.BaseUrl.TrimEnd('/');
    }

    public string? ToAbsoluteUrl(string? relativePath)
    {
        if (string.IsNullOrWhiteSpace(relativePath))
        {
            return null;
        }
        
        return $"{_baseUrl}/{relativePath.TrimStart('/')}";
    }
}