using System.ComponentModel.DataAnnotations;

namespace ReservationApp.Models
{
    public class Room
    {
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        public string Description { get; set; }

        [Required]
        public int RoomLevel { get; set; }

        //one Room can have more reservations
        public ICollection<Reservation> Reservations { get; set; }

    }
}
