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

        config.NewConfig<DisabilityType, DisabilityTypeResponse>();
        config.NewConfig<AccessibilityNeed, AccessibilityNeedResponse>();
    }
}
