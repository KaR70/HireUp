using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HireUp.Database.EntitiesConfiguration;

public class ReviewConfiguration : IEntityTypeConfiguration<Review>
{
    public void Configure(EntityTypeBuilder<Review> builder)
    {
        builder.HasKey(r => r.Id);
        
        builder.Property(r => r.Rating).IsRequired();
        builder.Property(r => r.Comment).HasMaxLength(2000);
        
        builder.HasOne(r => r.Author)
            .WithMany(u => u.ReviewsWritten)
            .HasForeignKey(r => r.AuthorId)
            .OnDelete(DeleteBehavior.Restrict);
        
        builder.HasOne(r => r.ReviewedUser)
            .WithMany(u => u.ReviewsReceived)
            .HasForeignKey(r => r.ReviewedUserId)
            .OnDelete(DeleteBehavior.Restrict);
        
        builder.HasOne(r => r.JobApplication)
            .WithMany()
            .HasForeignKey(r => r.JobApplicationId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}