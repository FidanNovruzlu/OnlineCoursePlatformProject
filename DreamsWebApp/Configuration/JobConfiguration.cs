using DreamsWebApp.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DreamsWebApp.Configuration;
public class JobConfiguration : IEntityTypeConfiguration<Job>
{
    public void Configure(EntityTypeBuilder<Job> builder)
    {
        builder.HasMany(c => c.Instructors)
                .WithOne(c => c.Job)
                .HasForeignKey(c => c.JobId)
                .OnDelete(DeleteBehavior.Restrict);
    }
}