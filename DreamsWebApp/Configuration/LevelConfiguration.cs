using DreamsWebApp.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DreamsWebApp.Configuration;

public class LevelConfiguration : IEntityTypeConfiguration<Level>
{
    public void Configure(EntityTypeBuilder<Level> builder)
    {
        builder.HasMany(c => c.Courses)
                .WithOne(c => c.Level)
                .HasForeignKey(c => c.LevelId)
                .OnDelete(DeleteBehavior.Restrict);
    }
}
