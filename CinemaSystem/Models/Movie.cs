using System.ComponentModel.DataAnnotations;

namespace CinemaSystem.Models
{
    public class Movie
    {
        public int Id { get; set; }
        [Required]
        public string Title { get; set; } = string.Empty;
        public string? Description { get; set; }
        [Required]
        public double Price { get; set; }
        public bool Status { get; set; }
        public DateTime DateTime { get; set; }
        public string? MainImg { get; set; }
        public int CategoryId { get; set; }
        public Category? Category { get; set; }
        public int CinemaId { get; set; }
        public Cinema? Cinema { get; set; }
        public List<MovieSubImg>? SubImages { get; set; }
        public List<MovieActor>? MovieActors { get; set; }
        

    }
}
