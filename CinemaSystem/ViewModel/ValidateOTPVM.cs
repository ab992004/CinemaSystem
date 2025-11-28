using System.ComponentModel.DataAnnotations;

namespace CinemaSystem.ViewModel
{
    public class ValidateOTPVM
    {
        public int Id { get; set; }

        [Required]
        public string OTP { get; set; } = string.Empty;
        public string UserId { get; set; } = string.Empty;
    }
}
