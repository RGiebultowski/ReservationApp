using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ReservationApp.Data;
using ReservationApp.Models;

namespace ReservationApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RoomsController : Controller
    {
        private readonly ApplicationDbContext context;

        public RoomsController(ApplicationDbContext context)
        {
            this.context = context;
        }

        //GET: api/Rooms
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Room>>> GetRooms()
        {
            var rooms = await context.Rooms.ToListAsync();

            if (rooms == null)
                return NotFound();

            return Ok(rooms);
        }

        //GET: api/Rooms/id
        [HttpGet("{id}")]
        public async Task<ActionResult<Room>> GetRoom(int id)
        {
            var room = await context.Rooms.FindAsync(id);
            
            if (room == null)
                return NotFound();

            return Ok(room);

            //cant use try catch due FindAsync does not throw an exception when it does not find any record
            //try
            //{
            //    return room;
            //}
            //catch (Exception e)
            //{
            //    return NotFound();
            //}
        }

        //POST: api/Rooms  adds new rooms to list
        [Authorize(Roles = "Administrator")]
        [HttpPost]
        public async Task<ActionResult<Room>> PostRoom(Room room)
        {
            context.Rooms.Add(room);
            await context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetRoom), new { id = room.Id }, room);
        }

        //PUT: api/Rooms/id  changes room detail
        [Authorize(Roles = "Administrator")]
        [HttpPut("{id}")]
        public async Task<IActionResult> PutRoom(int id, Room room)
        {
            if (id != room.Id)
                return BadRequest();

            context.Entry(room).State = EntityState.Modified;

            try
            {
                await context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!context.Rooms.Any(e => e.Id == id))
                    return NotFound();

                throw;
            }

            return NoContent();
        }

        // DELETE: api/Rooms/5
        [Authorize(Roles = "Administrator")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteRoom(int id)
        {
            var conferenceRoom = await context.Rooms.FindAsync(id);
            if (conferenceRoom == null)
                return NotFound();

            context.Rooms.Remove(conferenceRoom);
            await context.SaveChangesAsync();
            return NoContent();
        }
    }
}
