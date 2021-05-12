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

namespace WebApiProject.Controllers.V2
{
    [EnableCors("AnyOrigin")]
    [ApiVersion("2.0")]
    [Route("api/v{version:ApiVersion}/[controller]")]
    [ApiController]
    public class GeoMessagesControllerV2 : ControllerBase
    {
        private readonly GeoDbContext _context;
        private readonly SignInManager<MyUser> _signInManager;

        public GeoMessagesControllerV2(GeoDbContext context, SignInManager<MyUser> signInManager)
        {
            _context = context;
            _signInManager = signInManager;
        }

        // GET: api/GeoMessages
        [HttpGet("{id}")]
        public async Task<ActionResult<IEnumerable<GeoMessageV2>>> GetGeoMessages(int id)
        {
            var message = await _context.GeoMessagesV2.FindAsync(id);

            if (message == null)
                return NotFound();

            return Ok(message);
        }

        // GET: api/GeoMessages/5
        [HttpGet]
        public async Task<ActionResult<IEnumerable<GeoMessageV2>>> GetGeoMessage()
        {
            return await _context.GeoMessagesV2.ToListAsync();
        }

        // POST: api/GeoMessages
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [Authorize]
        [HttpPost]
        public async Task<ActionResult<GeoMessageV2>> PostGeoMessage(GeoMessageV2 geoMessage)
        {
            var newGeoMessage = new GeoMessageV2
            {
                Message = geoMessage.Message,
                Latitude = geoMessage.Latitude,
                Longitude = geoMessage.Longitude
            };

            await _context.AddAsync(newGeoMessage);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetGeoMessage", new { id = newGeoMessage.ID }, newGeoMessage);
        }
    }
}
