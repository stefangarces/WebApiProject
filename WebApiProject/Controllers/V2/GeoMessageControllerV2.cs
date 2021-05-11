﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApiProject.Data;
using WebApiProject.Models;

namespace WebApiProject.Controllers.V2
{
    [EnableCors("AnyOrigin")]
    [Route("api/V2/GetAllGeoMessages")]
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

            await _context.AddAsync(newGeoMessage);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetGeoMessage", new { id = newGeoMessage.ID }, newGeoMessage);
        }

        private bool GeoMessageExists(int id)
        {
            return _context.GeoMessages.Any(e => e.ID == id);
        }
    }
}
