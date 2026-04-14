namespace HireUp.Entities
{
    public class UserJobTypePreference
    {
        public string UserId { get; set; }
        public ApplicationUser User { get; set; }

        public int JobTypeId { get; set; }
        public JobType JobType { get; set; }
    }
}