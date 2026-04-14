namespace HireUp.Entities
{
    public class UserJobRolePreference
    {
        public string UserId { get; set; }
        public ApplicationUser User { get; set; }

        public int JobRoleId { get; set; }
        public JobRole JobRole { get; set; }
    }
}