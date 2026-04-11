namespace HireUp.Entities
{
    public class UserJobCategoryPreference
    {
        public string UserId { get; set; }
        public ApplicationUser User { get; set; }

        public int JobCategoryId { get; set; }
        public JobCategory JobCategory { get; set; }
    }
}