using HireUp.Entities;
using System.ComponentModel.DataAnnotations;

namespace HireUp.Entities
{
    public class MockInterview
    {
        public int Id { get; set; }

        [Required, MaxLength(200)]
        public string Title { get; set; } = string.Empty;

        [Required, MaxLength(100)]
        public string Industry { get; set; } = string.Empty;

        public DateTime ScheduledAt { get; set; }
        public int DurationMinutes { get; set; }
        public string JobSeekerId { get; set; }
        public string? InterviewerId { get; set; }
        public InterviewStatus Status { get; set; } = InterviewStatus.Scheduled;

        public string? Feedback { get; set; } 
        public int? Rating { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        // Navigation Properties
        public ApplicationUser JobSeeker { get; set; } = null!;      
        public ApplicationUser Interviewer { get; set; } = null!;    
    }

    public enum InterviewStatus
    {
        Scheduled, Completed, Cancelled, NoShow
    }
}