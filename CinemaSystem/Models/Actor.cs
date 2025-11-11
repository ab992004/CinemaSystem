using System.ComponentModel.DataAnnotations;

namespace CinemaSystem.Models
{
    public class Actor
    {
        public int Id { get; set; }
        [Required]
        public string Name { get; set; } = string.Empty;
        public string? Img { get; set; }
        public List<MovieActor>? MovieActors { get; set; }
    }
}
