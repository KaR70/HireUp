using HireUp.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HireUp.Database.EntitiesConfiguration;

public class SavedJobConfiguration : IEntityTypeConfiguration<SavedJob>
{
    public void Configure(EntityTypeBuilder<SavedJob> builder)
    {
        // تحديد المفتاح الأساسي
        builder.HasKey(sj => sj.Id);

        // إعداد العلاقة مع المستخدم
        builder.HasOne(sj => sj.User)
               .WithMany(u => u.SavedJobs)
               .HasForeignKey(sj => sj.UserId)
               .OnDelete(DeleteBehavior.Cascade); // لو اليوزر اتمسح، مسوداته تتمسح

        // إعداد العلاقة مع الوظيفة
        builder.HasOne(sj => sj.JobListing)
               .WithMany(j => j.SavedJobs)
               .HasForeignKey(sj => sj.JobListingId)
               .OnDelete(DeleteBehavior.Cascade); // لو الوظيفة اتمسحت، تتمسح من المحفوظات
    }
}