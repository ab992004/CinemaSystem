using CinemaSystem.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CinemaSystem.Data.Configurations
{
    public class MovieSubImgConfiguration : IEntityTypeConfiguration<MovieSubImg>
    {
        public void Configure(EntityTypeBuilder<MovieSubImg> builder)
        {
            builder.HasKey(msi => new { msi.MovieId, msi.Img });
            //builder.HasData(
            //    new MovieSubImg { MovieId = 1, Img = "inception_scene1.jpg" },
            //    new MovieSubImg { MovieId = 1, Img = "inception_scene2.jpg" },
            //    new MovieSubImg { MovieId = 2, Img = "dark_knight_scene1.jpg" },
            //    new MovieSubImg { MovieId = 2, Img = "dark_knight_scene2.jpg" },
            //    new MovieSubImg { MovieId = 3, Img = "interstellar_scene1.jpg" },
            //    new MovieSubImg { MovieId = 3, Img = "interstellar_scene2.jpg" },
            //    new MovieSubImg { MovieId = 4, Img = "godfather_scene1.jpg" },
            //    new MovieSubImg { MovieId = 4, Img = "godfather_scene2.jpg" }
            //);
        }
    }
}
