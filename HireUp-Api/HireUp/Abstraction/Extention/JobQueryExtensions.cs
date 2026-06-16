using HireUp.DTOs;

namespace HireUp.Abstraction.Extention;

public static class JobQueryExtensions
{
    public static IQueryable<JobListing> ApplyFilters(this IQueryable<JobListing> query, JobSearchFilterDto filters)
    {
        if (!string.IsNullOrEmpty(filters.SearchTerm))
            query = query.Where(j => j.Title.Contains(filters.SearchTerm) || j.Description.Contains(filters.SearchTerm));

        if (filters.AccessibilityNeedIds?.Any() == true)
            query = query.Where(j => j.JobAccessibilityNeeds.Any(jan => filters.AccessibilityNeedIds.Contains(jan.AccessibilityNeedId)));

        if (filters.ExperienceLevelId.HasValue)
            query = query.Where(j => j.ExperienceLevelId == filters.ExperienceLevelId);

        if (filters.JobTypeId.HasValue)
            query = query.Where(j => j.JobTypeId == filters.JobTypeId);
            
        if (filters.LocationId.HasValue)
            query = query.Where(j => j.LocationId == filters.LocationId);
        
        if (filters.OfficeTypeId.HasValue)
            query = query.Where(j => j.OfficeTypeId == filters.OfficeTypeId);

        if (filters.IndustryId.HasValue)
            query = query.Where(j => j.Company.IndustryId == filters.IndustryId);

        return query;
    }
}