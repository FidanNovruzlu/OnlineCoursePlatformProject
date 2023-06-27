using DreamsWebApp.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DreamsWebApp.Configuration;
public class LectureConfiguration : IEntityTypeConfiguration<Lecture>
{
    public void Configure(EntityTypeBuilder<Lecture> builder)
    {
        builder.HasOne(l => l.Section)
        .WithMany(s => s.Lectures)
        .HasForeignKey(l => l.SectionId)
        .OnDelete(DeleteBehavior.Restrict);
    }
}
