using CinemaSystem.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CinemaSystem.Data.Configurations
{
    public class ActorConfiguration : IEntityTypeConfiguration<Actor>
    {
        public void Configure(EntityTypeBuilder<Actor> builder)
        {
            builder.HasKey(a => a.Id);
            builder.Property(a => a.Name)
                   .IsRequired()
                   .HasMaxLength(100);
            //builder.HasData(
            //    new Actor { Id = 1, Name = "Robert Downey Jr.", Img = "robert_downey_jr.jpg" },
            //    new Actor { Id = 2, Name = "Scarlett Johansson", Img = "scarlett_johansson.jpg" },
            //    new Actor { Id = 3, Name = "Chris Hemsworth", Img = "chris_hemsworth.jpg" }
            //);
        }
    }
}
