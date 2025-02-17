using System.ComponentModel.DataAnnotations;

namespace ReservationApp.Models
{
    public class RegisterUser
    {
        [Required]
        public string Name { get; set; }

        [Required]
        [MinLength(5, ErrorMessage = "Password needs 5 chars!")]
        public string Password { get; set; }

        public bool IsAdmin { get; set; } = false;
    }
}
