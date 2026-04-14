using HireUp.DTOs.User;

namespace HireUp.Abstraction;

public interface ILookupService
{
    Task<UserPreferencesResponse> GetJobPreferencesAsync();

    Task<JobPreferencesLookupsResponse> GetJobPreferencesLookupsAsync();
}