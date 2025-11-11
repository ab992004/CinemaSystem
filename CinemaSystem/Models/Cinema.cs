using System.ComponentModel.DataAnnotations;

namespace CinemaSystem.Models
{
    public class Cinema
    {
        public int Id { get; set; }
        [Required]
        public string Name { get; set; } = string.Empty;
        [Required]
        public string Location { get; set; } = string.Empty;
        public List<Movie>? Movies { get; set; }
    }
}
