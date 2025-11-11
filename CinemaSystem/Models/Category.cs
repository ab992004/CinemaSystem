using System.ComponentModel.DataAnnotations;

namespace CinemaSystem.Models
{
    public class Category
    {
        public int Id { get; set; }
        [Required]
        public string Name { get; set; } = string.Empty;
        [MinLength(5)]
        public string? Description { get; set; }
        
    }
}
