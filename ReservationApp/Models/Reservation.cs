using System.ComponentModel.DataAnnotations;

namespace ReservationApp.Models
{
    public class Reservation
    {
        public int Id { get; set; }

        [Required]
        public Room RoomId { get; set; }

        [Required]
        public DateTime StartTime { get; set; }

        [Required]
        public DateTime EndTime { get; set; }

        // Reservation Person
        [Required]
        public string Organizer { get; set; }

        [Required]
        public string TitleOfMeeting { get; set; }

        public string Description { get; set; }

    }
}
