using DreamsWebApp.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DreamsWebApp.Configuration;

public class CourseConfiguration :IEntityTypeConfiguration<Course>
{
    public void Configure(EntityTypeBuilder<Course> builder)
    {
        builder.HasOne(c => c.Category)
            .WithMany(c=>c.Courses)
            .HasForeignKey(c => c.CategoryId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(c => c.Instructor)
            .WithMany(c=>c.Courses)
            .HasForeignKey(c => c.InstructorId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(c => c.Level)
            .WithMany(c=>c.Courses)
            .HasForeignKey(c => c.LevelId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
