using HireUp.Abstraction;
using HireUp.Database;
using HireUp.Database.Interfaces;
using HireUp.DTOs.Common;
using HireUp.DTOs.Industry;
using HireUp.DTOs.Location;
using HireUp.DTOs.User;
using Microsoft.EntityFrameworkCore;

namespace HireUp.Services;

public class LookupService : ILookupService
{
    private readonly ApplicationDbContext _context;
    private readonly IUnitOfWork _unitOfWork;

    public LookupService(ApplicationDbContext context, IUnitOfWork unitOfWork)
    {
        _context = context;
        _unitOfWork = unitOfWork;
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
    
        var officeTypes = await _context.OfficeTypes
            .Select(x => new LookupDto
            {
                Id = x.Id,
                Name = x.Name
            })
            .ToListAsync();
        
        var jobRoles = await _context.JobRoles
            .Select(x => new LookupDto
            {
                Id = x.Id,
                Name = x.Name
            })
            .ToListAsync();
        
        return new JobPreferencesLookupsResponse
        {
            JobRoles = jobRoles,
            Locations = locations,
            JobTypes = jobTypes,
            WorkModes = officeTypes
        };
    }

    public async Task<Result<IEnumerable<LocationSummaryResponse>>> GetLocationsAsync(CancellationToken cancellationToken = default)
    {
        var locations = await _unitOfWork.Locations.GetAllAsNoTrackingAsync(cancellationToken);

        if (!locations.Any())
            return Result.Failure<IEnumerable<LocationSummaryResponse>>(LocationErrors.NotFound);

        var response = locations.Adapt<IEnumerable<LocationSummaryResponse>>();
        
        return Result.Success(response);
    }
    
    public async Task<Result<IEnumerable<IndustryResponse>>> GetIndustriesAsync(CancellationToken cancellationToken = default)
    {
        var industries = await _unitOfWork.Industry.GetAllAsNoTrackingAsync(cancellationToken);

        if (!industries.Any())
            return Result.Failure<IEnumerable<IndustryResponse>>(IndustryErrors.NotFound);

        var response = industries.Adapt<IEnumerable<IndustryResponse>>();
        
        return Result.Success(response);
    }
}