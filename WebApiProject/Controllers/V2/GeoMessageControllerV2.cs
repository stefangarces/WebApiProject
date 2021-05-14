using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
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

        /// <summary>
        /// Här hämtas alla meddelanden inom det område som specificerats.
        /// </summary>
        /// <param name="minLon">Min Longitude för meddelanden</param>
        /// <param name="maxLon">Max longitude för meddelanden</param>
        /// <param name="minLat">Min Latitude för meddelanden</param>
        /// <param name="maxLat">Max latitude för meddelanden</param>
        /// <returns>Returnerar meddelanden i JSON-objekt</returns>

        // GET: api/GeoMessages
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<IEnumerable<GeoMessageV2>>> GetGeoMessage(int id)
        {
            var message = await _context.GeoMessagesV2.FindAsync(id);

            if (message == null)
                return NotFound();

            return Ok(message);
        }



        // GET: api/GeoMessages/5
        [HttpGet]
        public async Task<ActionResult<IEnumerable<GeoMessageV2>>> GetGeoMessages(double? minLon, double? maxLon, double? minLat, double? maxLat)
        {
            if (minLon == null || maxLon == null || minLat == null || maxLat == null)
            {
                var ListV1 = await _context.GeoMessages.ToListAsync();
                var ListV2 = await _context.GeoMessagesV2.ToListAsync();

                var megaList = formatV1(ListV1).Concat(formatV2(ListV2));
                return Ok(megaList);
            }
            else
            {
                var ListV1 = await _context.GeoMessages.Where(e => e.Latitude >= minLat && e.Latitude <= maxLat && e.Longitude >= minLon && e.Longitude <= maxLon).ToListAsync();
                var ListV2 = await _context.GeoMessagesV2.Where(e => e.Latitude >= minLat && e.Latitude <= maxLat && e.Longitude >= minLon && e.Longitude <= maxLon).ToListAsync();

                var megaList = formatV1(ListV1).Concat(formatV2(ListV2));
                return Ok(megaList);
            }
            // return await _context.GeoMessagesV2.ToListAsync();
        }




        private IEnumerable<GeoMessageV2_DTO> formatV1(IEnumerable<GeoMessage> list)
        {
            foreach (var message in list)
            {
                var messageDTO = new GeoMessageV2_DTO
                {
                    Message = new Messages { Title = String.Join(" ", message.Message.Split(" ").Take(3).Select((s, i) => i == 2 ? s.Substring(0, s.Length / 2) : s)), 
                    Body = message.Message, Author = "Bob" },
                    Longitude = message.Longitude,
                    Latitude = message.Latitude
                };

                yield return messageDTO;
            }
        }




        private IEnumerable<GeoMessageV2_DTO> formatV2(IEnumerable<GeoMessageV2> list)
        {
            foreach (var message in list)
            {
                var messageDTO = new GeoMessageV2_DTO
                {
                    Message = new Messages { Title = message.Title, Body = message.Body, Author = message.Author },
                    Longitude = message.Longitude,
                    Latitude = message.Latitude
                };
                yield return messageDTO;
            }
        }




        // POST: api/GeoMessages
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [Authorize]
        [EnableCors("AnyOrigin")]
        [HttpPost]
        public async Task<ActionResult<GeoMessageV2_DTO>> PostGeoMessage(GeoMessageV2_Input_DTO message)
        {
            var user = await _context.MyUsers.FindAsync(_userManager.GetUserId(User));
            var newMessage = new GeoMessageV2()
            {
                Title = message.Title,
                Body = message.Body,
                Author = $"{user.FirstName} {user.LastName}",
                Longitude = message.Longitude,
                Latitude = message.Latitude
            };
            await _context.AddAsync(newMessage);
            await _context.SaveChangesAsync();
            var messageDTO = new GeoMessageV2_DTO
            {
                Message = new Messages
                {
                    Body = newMessage.Body,
                    Title = newMessage.Title,
                    Author = newMessage.Author
                },
                Longitude = newMessage.Longitude,
                Latitude = newMessage.Latitude
            };

            return CreatedAtAction(nameof(GetGeoMessage), new { id = newMessage.Id }, messageDTO);
        }
    }
}
