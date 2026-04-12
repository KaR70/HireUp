namespace HireUp.Entities;

public class ApplicationUser : IdentityUser
{
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string? Bio { get; set; }
    public string? ProfilePicture { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public bool IsActive { get; set; } = true;
    public string? PasswordResetCode { get; set; }
    public DateTime? PasswordResetCodeExpiry { get; set; }
    public DateOnly? Birthday { get; set; }
    public Gender? Gender { get; set; }
    public string? Header { get; set; }

    public int? LocationId { get; set; }
    public int? JobRoleId { get; set; }

    public Location Location { get; set; }
    public JobRole? JobRole { get; set; }  
    
    public ICollection<Skill> Skills { get; set; } = new List<Skill>();
    public ICollection<JobListing> JobListings { get; set; } = new List<JobListing>();
    public ICollection<MockInterview> InterviewsAsSeeker { get; set; } = new List<MockInterview>();
    public ICollection<MockInterview> InterviewsAsInterviewer { get; set; } = new List<MockInterview>();
    public ICollection<JobApplication> Applications { get; set; } = new List<JobApplication>();
    public ICollection<Follows> Followers { get; set; } = new List<Follows>();
    public ICollection<Follows> Following { get; set; } = new List<Follows>();
    public ICollection<Review> ReviewsWritten { get; set; } = new List<Review>();
    public ICollection<Review> ReviewsReceived { get; set; } = new List<Review>();
    public virtual ICollection<UserDisabilityType> UserDisabilityTypes { get; set; } = new List<UserDisabilityType>();
    public virtual ICollection<UserAccessibilityNeed> UserAccessibilityNeeds { get; set; } = new List<UserAccessibilityNeed>();
    public List<RefreshToken> RefreshTokens { get; set; } = [];

    public virtual ICollection<UserJobTypePreference> UserJobTypePreferences { get; set; } = new HashSet<UserJobTypePreference>();
    public virtual ICollection<UserLocationPreference> UserLocationPreferences { get; set; } = new HashSet<UserLocationPreference>();
    public virtual ICollection<UserOfficeTypePreference> UserOfficeTypePreferences { get; set; } = new HashSet<UserOfficeTypePreference>();
    public virtual ICollection<UserJobCategoryPreference> UserJobCategoryPreferences { get; set; } = new HashSet<UserJobCategoryPreference>();
}

public enum Gender
{
    Male,
    Female
}


