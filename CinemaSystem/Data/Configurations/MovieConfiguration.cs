using CinemaSystem.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
namespace CinemaSystem.Data.Configurations
{
    public class MovieConfiguration : IEntityTypeConfiguration<Movie>
    {
        public void Configure(EntityTypeBuilder<Movie> builder)
        {
            builder.HasKey(m => m.Id);
            builder.HasMany(m => m.SubImages)
                   .WithOne(si => si.Movie)
                   .HasForeignKey(si => si.MovieId);
            //builder.HasData(
            //    new Movie
            //    {
            //        Id = 1,
            //        Title = "Inception",
            //        Description = "A skilled thief is offered a chance to erase his criminal record by performing inception.",
            //        Price = 150.0,
            //        Status = true,
            //        DateTime = new DateTime(2025, 11, 15, 20, 0, 0),
            //        MainImg = "inception.jpg",
            //        CategoryId = 1, 
            //        CinemaId = 1
            //    },
            //    new Movie
            //    {
            //        Id = 2,
            //        Title = "The Dark Knight",
            //        Description = "Batman faces the Joker, a criminal mastermind who seeks to plunge Gotham into anarchy.",
            //        Price = 180.0,
            //        Status = true,
            //        DateTime = new DateTime(2025, 11, 20, 21, 30, 0),
            //        MainImg = "dark_knight.jpg",
            //        CategoryId = 2, 
            //        CinemaId = 2
            //    },
            //    new Movie
            //    {
            //        Id = 3,
            //        Title = "Interstellar",
            //        Description = "A team of explorers travel through a wormhole in space in an attempt to ensure humanity’s survival.",
            //        Price = 200.0,
            //        Status = true,
            //        DateTime = new DateTime(2025, 11, 25, 19, 0, 0),
            //        MainImg = "interstellar.jpg",
            //        CategoryId = 1, 
            //        CinemaId = 3
            //    },
            //    new Movie
            //    {
            //        Id = 4,
            //        Title = "The Godfather",
            //        Description = "The aging patriarch of an organized crime dynasty transfers control to his reluctant son.",
            //        Price = 120.0,
            //        Status = false,
            //        DateTime = new DateTime(2025, 12, 1, 22, 0, 0),
            //        MainImg = "godfather.jpg",
            //        CategoryId = 3, 
            //        CinemaId = 2,
            //    },
            //    new Movie
            //    {
            //        Id = 5,
            //        Title = "Avatar: The Way of Water",
            //        Description = "Jake Sully and Ney'tiri have formed a family and must explore new regions of Pandora.",
            //        Price = 210.0,
            //        Status = true,
            //        DateTime = new DateTime(2025, 12, 5, 18, 30, 0),
            //        MainImg = "avatar2.jpg",
            //        CategoryId = 3,
            //        CinemaId = 1
            //    }
            //    );
        }
    }
}
