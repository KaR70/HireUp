namespace HireUp.Entities
{
    public class UserLocationPreference
    {
        public string UserId { get; set; }
        public ApplicationUser User { get; set; }

        public int LocationId { get; set; }
        public Location Location { get; set; }
    }
}
