using HireUp.Entities;
using System.ComponentModel.DataAnnotations;

namespace HireUp.Entities
{
    public class JobApplication
    {
        public int Id { get; set; }
        public int JobListingId { get; set; }
        public string JobSeekerId { get; set; }

        [Required]
        public string CoverLetter { get; set; } = string.Empty;  

        public ApplicationStatus Status { get; set; } = ApplicationStatus.Pending;
        public DateTime AppliedAt { get; set; } = DateTime.UtcNow;

        public string? Notes { get; set; } 

        // Navigation Properties
        public JobListing JobListing { get; set; } = null!;  
        public ApplicationUser JobSeeker { get; set; } = null!;        
    }

    public enum ApplicationStatus
    {
        Pending, Reviewed, Accepted, Rejected
    }
}