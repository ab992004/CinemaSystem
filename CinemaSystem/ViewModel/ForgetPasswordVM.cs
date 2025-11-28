using System.ComponentModel.DataAnnotations;

namespace CinemaSystem.ViewModel
{
    public class ForgetPasswordVM
    {
        public int Id { get; set; }

        [Required]
        public string UserNameOrEmail { get; set; } = string.Empty;
    }
}
