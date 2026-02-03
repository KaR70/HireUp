using HireUp.DTOs.JobListing;

namespace HireUp.Mapping;

public class MappingConfigurations : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.NewConfig<ApplicationUser, MyProfileResponse>()
            .Map(dest => dest.UserId, src => src.Id)
            .Map(dest => dest.DisabilityTypes, src => src.UserDisabilityTypes.Select(udt => udt.DisabilityType))
            .Map(dest => dest.AccessibilityNeeds, src => src.UserAccessibilityNeeds.Select(uan => uan.AccessibilityNeed))
            .Map(dest => dest.ProfilePictureUrl, src => src.ProfilePicture);

        config.NewConfig<ApplicationUser, PublicProfileResponse>()
            .Map(dest => dest.ProfilePicture, src => src.ProfilePicture)
            .Map(dest => dest.UserId, src => src.Id);
        
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
                src.Type.ToString(),
                src.ExperienceLevel != null ? src.ExperienceLevel.Name : "Any Level",
                src.JobCategory != null ? src.JobCategory.Name : "General"
            })
            
            .Include<JobListing, JobListingDetailResponse>();
        
        config.NewConfig<JobListing, JobListingDetailResponse>()
            .Map(dest => dest.AboutCompany, src => src.Company != null ? src.Company.Description : "Company information not available.");

        config.NewConfig<DisabilityType, DisabilityTypeResponse>();
        config.NewConfig<AccessibilityNeed, AccessibilityNeedResponse>();
    }
}
