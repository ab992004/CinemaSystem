using System.ComponentModel.DataAnnotations;

namespace CinemaSystem.Models
{
    public class MovieSubImg
    {
        public int MovieId { get; set; }
        public Movie? Movie { get; set; }
        [Required]
        public string Img { get; set; } = string.Empty;
    }
}
