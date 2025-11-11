using CinemaSystem.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
namespace CinemaSystem.Data.Configurations
{
    public class CategoryConfiguration: IEntityTypeConfiguration<Category>
    {
        public void Configure(EntityTypeBuilder<Category> builder)
        {
            builder.HasKey(c => c.Id);
            builder.Property(c => c.Name)
                   .IsRequired()
                   .HasMaxLength(100);
            builder.Property(c => c.Description)
                   .HasMaxLength(500);
            //builder.HasData(
            //    new Category { Id = 1, Name = "Action", Description = "Action-packed movies with thrilling sequences." },
            //    new Category { Id = 2, Name = "Comedy", Description = "Light-hearted movies designed to entertain and amuse." },
            //    new Category { Id = 3, Name = "Drama", Description = "Movies that focus on emotional and relational development." }
            //);
        }

    }
}
