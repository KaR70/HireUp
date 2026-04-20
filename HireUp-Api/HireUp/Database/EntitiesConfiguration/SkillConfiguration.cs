using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HireUp.Database.EntitiesConfiguration;

public class SkillConfiguration : IEntityTypeConfiguration<Skill>
{
    public void Configure(EntityTypeBuilder<Skill> builder)
    {
        builder.HasData(
            new Skill
            {
                Id = 1,
                Name = "C#",
                Description = "Backend development",
                Category = SkillCategory.Technical
            },
            new Skill
            {
                Id = 2,
                Name = "React",
                Description = "Frontend development",
                Category = SkillCategory.Technical
            },
            new Skill
            {
                Id = 3,
                Name = "UI/UX",
                Description = "Design Skills",
                Category = SkillCategory.Professional
            }
        );
    }
}