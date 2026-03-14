using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HireUp.Entities
{
    public class JobApplication
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int JobListingId { get; set; }

        [ForeignKey("JobListingId")]
        public virtual JobListing JobListing { get; set; } = null!;

        [Required]
        public string JobSeekerId { get; set; }

        [ForeignKey("JobSeekerId")]
        public virtual ApplicationUser JobSeeker { get; set; } = null!;

        [Required]
        public string CoverLetter { get; set; } = string.Empty;

        [Required]
        public string ResumeUrl { get; set; } = string.Empty;

        public ApplicationStatus Status { get; set; } = ApplicationStatus.Pending;
        public DateTime AppliedAt { get; set; } = DateTime.UtcNow;
        public string? Notes { get; set; }
    }

    public enum ApplicationStatus
    {
        Pending,
        Reviewed,
        Accepted,
        Rejected
    }
}