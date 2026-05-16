using HireUp.DTOs;
using HireUp.DTOs.Company;
using HireUp.DTOs.Disabled;
using HireUp.DTOs.JobListing;

namespace HireUp.Mapping;

public class MappingConfigurations : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.NewConfig<ApplicationUser, ProfileHeaderResponse>()
            .Map(dest => dest.FullName, src => $"{src.FirstName} {src.LastName}");
        
        config.NewConfig<ApplicationUser, MyProfileResponse>()
            .Map(dest => dest.UserId, src => src.Id)
            .Map(dest => dest.ProfilePictureUrl, src => src.ProfilePicture)
            .Map(dest => dest.FullName, src => $"{src.FirstName} {src.LastName}")
            .Map(dest => dest.Gender, src => src.Gender.HasValue ? src.Gender.Value.ToString() : null)
            .Map(dest => dest.Location, src => src.Location)
            .Map(dest => dest.JobRole, src => src.JobRole);

        config.NewConfig<ApplicationUser, PublicProfileResponse>()
            .Map(dest => dest.ProfilePicture, src => src.ProfilePicture)
            .Map(dest => dest.UserId, src => src.Id)
            .Map(dest => dest.FullName, src => $"{src.FirstName} {src.LastName}")
            .Map(dest => dest.AboutMe, src  => src.Bio);
        
        config.NewConfig<RegisterRequest, ApplicationUser>()
            .Map(dest => dest.UserName, src => src.Email);

        config.NewConfig<JobListing, JobListingSummaryResponse>()
            .Map(dest => dest.CompanyName, src => src.Company != null ? src.Company.Name : "N/A")
            .Map(dest => dest.CompanyLogoUrl, src => src.Company != null ? src.Company.Logo : string.Empty)

            .Map(dest => dest.SalaryDisplay, src => src.Salary.HasValue
                ? $"{src.Salary.Value:C0}/year"
                : "Salary not specified")

            .Map(dest => dest.Tags, src => new List<string>
            {
                src.JobType.Name,
                src.ExperienceLevel != null ? src.ExperienceLevel.Name : "Any Level",
                src.JobCategory != null ? src.JobCategory.Name : "General"
            })
            
            .Include<JobListing, JobListingDetailResponse>();
        
        config.NewConfig<JobListing, JobListingDetailResponse>()
            .Map(dest => dest.AboutCompany, src => src.Company != null ? src.Company.Description : "Company information not available.");

        config.NewConfig<Company, RegisterProfileRequest>()
            .Map(dest => dest.FoundedYear, src => src.FoundedYear);
        
        config.NewConfig<RegisterProfileRequest, Company>()
            .Map(dest => dest.FoundedYear, src => src.FoundedYear);
        
        config.NewConfig<Company, ProfileResponse>()
            .Map(dest => dest.Headquarters, src => src.Location);

        config.NewConfig<JobApplication, RecentApplicantDto>()
            .Map(dest => dest.ApplicationId, src => src.Id)
            .Map(dest => dest.ApplicantId, src => src.JobSeekerId)
            .Map(dest => dest.ApplicantName, src => $"{src.JobSeeker.FirstName} {src.JobSeeker.LastName}")
            .Map(dest => dest.JobRole, src => src.JobListing.Title)
            .Map(dest => dest.ApplicantProfilePictureUrl, src => src.JobSeeker.ProfilePicture);

        config.NewConfig<JobListing, CompanyJobSummaryResponse>()
            .Map(dest => dest.PostedAt, src => src.CreatedAt)
            .Map(dest => dest.ApplicantsCount, src => src.Applications.Count);
        
        config.NewConfig<DisabilityType, DisabilityTypeResponse>();
        config.NewConfig<AccessibilityNeed, AccessibilityNeedResponse>();
        config.NewConfig<Skill, SkillResponse>();
    }
}
