using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HireUp.Database.EntitiesConfiguration;

public class DisabilityTypeConfiguration : IEntityTypeConfiguration<DisabilityType>
{
    public void Configure(EntityTypeBuilder<DisabilityType> builder)
    {
        builder.HasKey(dt => dt.Id);

        builder.Property(dt => dt.Name)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(dt => dt.Description)
            .HasMaxLength(250);

        builder.HasMany(dt => dt.UserDisabilityTypes)
            .WithOne(udt => udt.DisabilityType)
            .HasForeignKey(udt => udt.DisabilityTypeId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasData([
            new DisabilityType 
            { 
                Id = 1, 
                Name = "Visual Disability", 
                Description = "Candidates who are blind, have low vision, or experience color-blindness and utilize assistive visual tools or screen modifications." 
            },
            new DisabilityType 
            { 
                Id = 2, 
                Name = "Hearing or Auditory Disability", 
                Description = "Candidates who are deaf or hard-of-hearing and rely on written communication channels or captioning frameworks." 
            },
            new DisabilityType 
            { 
                Id = 3, 
                Name = "Physical or Mobility Disability", 
                Description = "Candidates with limited fine motor skills, repetitive strain injuries, or mobility variations requiring ergonomic or alternative physical/digital access." 
            },
            new DisabilityType 
            { 
                Id = 4, 
                Name = "Neurodivergence & Cognitive Variations", 
                Description = "Candidates with ADHD, Autism, Dyslexia, or processing variations who thrive with structured workflows, clear communication, or specialized environments." 
            },
            new DisabilityType 
            { 
                Id = 5, 
                Name = "Chronic Illness or Invisible Disability", 
                Description = "Candidates managing long-term health conditions (e.g., autoimmune, chronic pain) requiring energy management or medical schedule flexibility." 
            }
        ]);
    }
}
