using System.ComponentModel.DataAnnotations;

namespace ReservationApp.Models
{
    public class LoginUser
    {
        [Required]
        public string Name { get; set; }

        [Required]
        public string Password { get; set; }
    }
}
