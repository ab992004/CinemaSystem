using CinemaSystem.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CinemaSystem.Data.Configurations
{
    public class CinemaConfiguration : IEntityTypeConfiguration<Cinema>
    {
        public void Configure(EntityTypeBuilder<Cinema> builder)
        {
            builder.HasKey(c => c.Id);
            builder.Property(c => c.Name)
                   .IsRequired()
                   .HasMaxLength(100);
            builder.Property(c => c.Location)
                   .IsRequired()
                   .HasMaxLength(200);
            builder.HasMany(c => c.Movies)
                     .WithOne(m => m.Cinema)
                     .HasForeignKey(m => m.CinemaId);
            //builder.HasData(
            //    new Cinema { Id = 1, Name = "Grand Cinema", Location = "Downtown" },
            //    new Cinema { Id = 2, Name = "Movie Palace", Location = "Uptown" },
            //    new Cinema { Id = 3, Name = "Cineplex 21", Location = "Suburbs" }
            //);
        }
    }
}
