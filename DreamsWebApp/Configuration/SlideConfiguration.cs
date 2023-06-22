using DreamsWebApp.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DreamsWebApp.Configuration
{
    public class SlideConfiguration : IEntityTypeConfiguration<Slide>
    {
        public void Configure(EntityTypeBuilder<Slide> builder)
        {
            builder.HasOne(s => s.Category)
                .WithMany(c => c.Slides)
                .HasForeignKey(s => s.CatagoryId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
