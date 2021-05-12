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
        private readonly UserManager<MyUser> _userManager;

        public GeoMessagesControllerV2(GeoDbContext context, SignInManager<MyUser> signInManager, UserManager<MyUser> userManager)
        {
            _context = context;
            _signInManager = signInManager;
            _userManager = userManager;
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
        [EnableCors("AnyOrigin")]
        [HttpPost]
        public async Task<ActionResult<GeoMessageV2>> PostGeoMessage(GeoMessageV2_DTO geoMessage)
        {
            var getUserId = _userManager.GetUserId(User);
            var getUser = await _context.MyUsers.Where(i => i.Id == getUserId).FirstOrDefaultAsync();

            var geoMessageV2DTO = new GeoMessageV2_DTO
            {
                Title = geoMessage.Title,
                Body = geoMessage.Body,
                Longitude = geoMessage.Longitude,
                Latitude = geoMessage.Latitude
            };

            var returnMessageV2 = new Messages
            {
                Message = new MessageDTO
                {
                    Title = geoMessageV2DTO.Title,
                    Author = getUser.FirstName + " " + getUser.LastName,
                    Body = geoMessageV2DTO.Body
                },
                Latitude = geoMessage.Latitude,
                Longitude = geoMessage.Longitude
            };

            var newGeoMessage = new GeoMessage
            {
                Message = geoMessage.Body,
                Latitude = returnMessageV2.Latitude,
                Longitude = returnMessageV2.Longitude
            };

            var newGeoMessageV2 = new GeoMessageV2
            {
                Title = geoMessageV2DTO.Title,
                Body = geoMessageV2DTO.Body,
                Author = getUser.FirstName + " " + getUser.LastName,
                Latitude = geoMessageV2DTO.Latitude,
                Longitude = geoMessageV2DTO.Longitude,
            };

            await _context.AddAsync(newGeoMessage);
            await _context.AddAsync(newGeoMessageV2);
            await _context.SaveChangesAsync();

            return newGeoMessageV2;
        }
    }
}
