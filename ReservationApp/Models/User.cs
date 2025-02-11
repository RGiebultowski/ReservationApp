using System.ComponentModel.DataAnnotations;

namespace ReservationApp.Models
{
    public class User
    {
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public bool IsAdministrator { get; set; }
    }
}
