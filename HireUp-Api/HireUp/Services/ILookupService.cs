namespace HireUp.Abstraction;

public interface ILookupService
{
    Task<JobPreferencesLookupsResponse> GetJobPreferencesLookupsAsync();
    Task<Result<IEnumerable<LocationSummaryResponse>>> GetLocationsAsync(CancellationToken cancellationToken = default);
    Task<Result<IEnumerable<IndustryResponse>>> GetIndustriesAsync(CancellationToken cancellationToken = default);

}