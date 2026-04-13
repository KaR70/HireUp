using System.ComponentModel.DataAnnotations;

namespace HireUp.Entities
{
    public class SavedJob
    {
        public int Id { get; set; }

        public string UserId { get; set; }
        public ApplicationUser User { get; set; }
        public int JobListingId { get; set; }
        public JobListing JobListing { get; set; }

        public DateTime SavedAt { get; set; } = DateTime.UtcNow;
    }
}