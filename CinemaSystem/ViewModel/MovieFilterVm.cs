using CinemaSystem.Models;

namespace CinemaSystem.ViewModel
{
    public record ActorCategoryVm
    {
           public IEnumerable<Actor> Actors { get; set; } = null!;
        public IEnumerable<Category> Categories { get; set; } = null!;
    }
}
