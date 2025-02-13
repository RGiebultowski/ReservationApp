using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ReservationApp.Data;
using ReservationApp.Models;

namespace ReservationApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReservationsController : Controller
    {
        private readonly ApplicationDbContext context;

        public ReservationsController(ApplicationDbContext context)
        {
            this.context = context;
        }

        // GET: api/Reservations
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Reservation>>> GetReservations()
        {
            var reservations = await context.Reservations.Include(room => room.RoomId).ToListAsync();
            if (reservations == null)
                return NotFound();

            return Ok(reservations);
        }

        // GET: api/Reservations/id
        [HttpGet("{id}")]
        public async Task<ActionResult<Reservation>> GetReservation(int id)
        {
            var reservation = await context.Reservations.Include(room => room.RoomId).FirstOrDefaultAsync(reservation => reservation.Id == id);
            if (reservation == null)
                return NotFound();

            return Ok(reservation);
        }

        // POST: api/Reservations
        [HttpPost]
        public async Task<ActionResult<Reservation>> PostReservation(Reservation reservation)
        {
            //collision check
            var conflict = await context.Reservations.AnyAsync(
                room => room.RoomId == reservation.RoomId &&
                //Checks if the existing reservation starts before the end of the new one.
                room.StartTime < reservation.EndTime &&
                //Checks if the new reservation starts before the end of the existing one.
                reservation.StartTime < room.EndTime);

            if (conflict)
                return BadRequest($"In this time room {reservation.RoomId.Name} is reserved.");
            else
                context.Reservations.Add(reservation);
            await context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetReservation), new {id = reservation.Id}, reservation);
        }

        // PUT: api/Reservations/id
        [HttpPut("{id}")]
        public async Task<IActionResult> PutReservation(int id, Reservation reservation)
        {
            if (id != reservation.Id)
                return BadRequest();

            // collision check
            bool conflict = await context.Reservations.AnyAsync(r =>
                r.Id != reservation.Id &&
                r.RoomId == reservation.RoomId &&
                r.StartTime < reservation.EndTime &&
                reservation.StartTime < r.EndTime);

            if (conflict)
                return BadRequest($"In this time room {reservation.RoomId.Name} is reserved.");

            context.Entry(reservation).State = EntityState.Modified;

            try
            {
                await context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!context.Reservations.Any(e => e.Id == id))
                    return NotFound();
                throw;
            }

            return NoContent();
        }

        // DELETE: api/Reservations/id
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteReservation(int id)
        {
            var reservation = await context.Reservations.FindAsync(id);
            if (reservation == null)
                return NotFound();

            context.Reservations.Remove(reservation);
            await context.SaveChangesAsync();
            return NoContent();
        }

    }
}
