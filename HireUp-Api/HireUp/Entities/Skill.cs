using HireUp.Entities;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace HireUp.Entities
{
    public class Skill
    {
        public int Id { get; set; }

        [Required, MaxLength(100)]
        public string Name { get; set; } = string.Empty;

        public string Description { get; set; } = string.Empty;

        public SkillCategory Category { get; set; }

        public string? IconUrl { get; set; }  

        public ICollection<ApplicationUser> Users { get; set; } = new List<ApplicationUser>();
        public ICollection<JobListing> JobListings { get; set; } = new List<JobListing>();
    }

    public enum SkillCategory
    {
        Technical, Manual, HomeBased, Craft, Educational, Professional
    }
}