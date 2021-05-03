using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApiProject.Data;
using WebApiProject.Models;

namespace WebApiProject.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GeoMessagesController : ControllerBase
    {
        private readonly GeoDbContext _context;

        public GeoMessagesController(GeoDbContext context)
        {
            _context = context;
        }

        // GET: api/GeoMessages
        [HttpGet("/v1/geo-comments/{id}")]
        public async Task<ActionResult<IEnumerable<GeoMessage>>> GetGeoMessages(int id)
        {
            var message = await _context.GeoMessages.FindAsync(id);

            if (message == null)
                return NotFound();

            return Ok(message);
        }

        // GET: api/GeoMessages/5
        [HttpGet("/v1/geo-comments")]
        public async Task<ActionResult<IEnumerable<GeoMessage>>> GetGeoMessage()
        {
            return await _context.GeoMessages.ToListAsync();
        }

        // PUT: api/GeoMessages/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutGeoMessage(int id, GeoMessage geoMessage)
        {
            if (id != geoMessage.ID)
            {
                return BadRequest();
            }

            _context.Entry(geoMessage).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!GeoMessageExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/GeoMessages
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost("/v1/geo-comments")]
        public async Task<ActionResult<GeoMessage>> PostGeoMessage(GeoMessage geoMessage)
        {
            var newGeoMessage = new GeoMessage
            {
                Message = geoMessage.Message,
                Latitude = geoMessage.Latitude,
                Longitude = geoMessage.Longitude
            };

            await _context.AddAsync(newGeoMessage);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetGeoMessage", new { id = newGeoMessage.ID }, newGeoMessage);
        }

        // DELETE: api/GeoMessages/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteGeoMessage(int id)
        {
            var geoMessage = await _context.GeoMessages.FindAsync(id);
            if (geoMessage == null)
            {
                return NotFound();
            }

            _context.GeoMessages.Remove(geoMessage);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool GeoMessageExists(int id)
        {
            return _context.GeoMessages.Any(e => e.ID == id);
        }
    }
}
