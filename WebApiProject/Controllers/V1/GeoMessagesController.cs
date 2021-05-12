using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApiProject.Data;
using WebApiProject.Models;
using WebApiProject.Models.V2;

namespace WebApiProject.Controllers
{
    [EnableCors("AnyOrigin")]
    [ApiVersion("1.0")]
    [Route("api/v{version:ApiVersion}/[controller]")]
    [ApiController]
    public class GeoMessagesController : ControllerBase
    {
        private readonly GeoDbContext _context;
        private readonly SignInManager<MyUser> _signInManager;

        public GeoMessagesController(GeoDbContext context, SignInManager<MyUser> signInManager)
        {
            _context = context;
            _signInManager = signInManager;
        }

        // GET: api/GeoMessages
        [HttpGet("{id}")]
        public async Task<ActionResult<IEnumerable<GeoMessage>>> GetGeoMessages(int id)
        {
            var message = await _context.GeoMessages.FindAsync(id);

            if (message == null)
                return NotFound();

            return Ok(message);
        }

        // GET: api/GeoMessages/5
        [HttpGet]
        public async Task<ActionResult<IEnumerable<GeoMessage>>> GetGeoMessage()
        {
            return await _context.GeoMessages.ToListAsync();
        }

        // POST: api/GeoMessages
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [Authorize]
        [HttpPost]
        public async Task<ActionResult<GeoMessage>> PostGeoMessage(GeoMessage geoMessage)
        {
            var newGeoMessage = new GeoMessage
            {
                Message = geoMessage.Message,
                Latitude = geoMessage.Latitude,
                Longitude = geoMessage.Longitude
            };

            var geoMessageV2DTO = new Models.V1.GeoMessageV1_DTO
            {
                Message = newGeoMessage.Message,
                Latitude = newGeoMessage.Latitude,
                Longitude = newGeoMessage.Longitude
            };

            var newGeoMessageV2 = new GeoMessageV2
            {
                Latitude = geoMessageV2DTO.Latitude,
                Longitude = geoMessageV2DTO.Longitude
            };


            await _context.AddAsync(newGeoMessage);
            await _context.AddAsync(newGeoMessageV2);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetGeoMessage", new { id = newGeoMessage.ID }, newGeoMessage);
        }

        private bool GeoMessageExists(int id)
        {
            return _context.GeoMessages.Any(e => e.ID == id);
        }
    }
}
