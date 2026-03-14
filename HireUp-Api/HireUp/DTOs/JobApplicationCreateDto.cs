using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace HireUp.DTOs
{
    public class JobApplicationCreateDto
    {
        [Required]
        public int JobListingId { get; set; }

        [Required]
        public string CoverLetter { get; set; } = string.Empty;

        [Required]
        public IFormFile ResumeFile { get; set; } 
    }
}