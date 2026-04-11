using HireUp.Abstraction;
using HireUp.Database;
using HireUp.DTOs.Common;
using HireUp.DTOs.User;
using Microsoft.EntityFrameworkCore;

namespace HireUp.Services;

public class LookupService : ILookupService
{
    private readonly ApplicationDbContext _context;

    public LookupService(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<JobPreferencesLookupsResponse> GetJobPreferencesLookupsAsync()
    {
        var jobTypes = await _context.JobTypes
            .Select(x => new LookupDto
            {
                Id = x.Id,
                Name = x.Name
            })
            .ToListAsync();

        var locations = await _context.Locations
        .Select(l => new LookupDto
        {
            Id = l.Id,
            Name = l.City
        })
    .ToListAsync();

        return new JobPreferencesLookupsResponse
        {
            JobTypes = jobTypes,
            Locations = locations
        };
    }

    public async Task<UserPreferencesResponse> GetJobPreferencesAsync()
    {
        throw new NotImplementedException();
    }
}