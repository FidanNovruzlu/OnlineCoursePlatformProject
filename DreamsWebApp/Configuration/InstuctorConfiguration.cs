using DreamsWebApp.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DreamsWebApp.Configuration;

public class InstuctorConfiguration : IEntityTypeConfiguration<Instructor>
{
    public void Configure(EntityTypeBuilder<Instructor> builder)
    {
        builder.HasMany(c => c.Courses)
                .WithOne(c => c.Instructor)
                .HasForeignKey(c => c.InstructorId)
                .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(c => c.Job)
                .WithMany(c => c.Instructors)
                .HasForeignKey(c => c.JobId)
                .OnDelete(DeleteBehavior.Restrict);
    }
}
