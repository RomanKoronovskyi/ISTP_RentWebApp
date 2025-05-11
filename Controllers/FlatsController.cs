using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RentAPIWebApp.Models;

namespace RentAPIWebApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FlatsController : ControllerBase
    {
        private readonly RentAPIContext _context;

        public FlatsController(RentAPIContext context)
        {
            _context = context;
        }

        // GET: api/Flats
        [HttpGet]
        public async Task<ActionResult<IEnumerable<object>>> GetFlats()
        {
            var flats = await _context.Flats
                .Include(f => f.Ds)
                .Select(f => new {
                    f.Id,
                    f.FlAddr,
                    f.FlArea,
                    f.FlRooms,
                    f.FlPrice,
                    districtId = f.DsId,
                    districtName = f.Ds.DsName
                })
                .ToListAsync();

            return Ok(flats);
        }

        // GET: api/Flats/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Flats>> GetFlats(int id)
        {
            var flats = await _context.Flats.FindAsync(id);

            if (flats == null)
            {
                return NotFound();
            }

            return flats;
        }

        // PUT: api/Flats/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutFlats(int id, Flats flats)
        {
            if (id != flats.Id)
            {
                return BadRequest();
            }

            _context.Entry(flats).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!FlatsExists(id))
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

        // POST: api/Flats
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<IActionResult> PostFlats([FromBody] Flats flat)
        {
            var district = await _context.Districts.FindAsync(flat.DsId);
            if (district == null)
                return NotFound("District not found");

            _context.Flats.Add(flat);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetFlats), new { id = flat.Id }, flat);
        }

        // DELETE: api/Flats/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteFlats(int id)
        {
            var flats = await _context.Flats.FindAsync(id);
            if (flats == null)
            {
                return NotFound();
            }

            _context.Flats.Remove(flats);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool FlatsExists(int id)
        {
            return _context.Flats.Any(e => e.Id == id);
        }
        [HttpPatch("{id}")]
        public async Task<IActionResult> PatchFlats(int id, [FromBody] JsonElement updates)
        {
            var flat = await _context.Flats.FindAsync(id);
            if (flat == null)
            {
                return NotFound();
            }

            var json = updates.GetRawText();
            var patchDoc = JsonSerializer.Deserialize<Dictionary<string, JsonElement>>(json);

            foreach (var entry in patchDoc)
            {
                var property = typeof(Flats).GetProperty(entry.Key);
                if (property != null)
                {
                    var value = Convert.ChangeType(entry.Value.ToString(), property.PropertyType);
                    property.SetValue(flat, value);
                }
            }

            await _context.SaveChangesAsync();
            return NoContent();
        }

        // HEAD: api/Flats/5
        [HttpHead("{id}")]
        public async Task<IActionResult> HeadFlats(int id)
        {
            var exists = await _context.Flats.AnyAsync(f => f.Id == id);

            if (!exists)
            {
                return NotFound();
            }

            Response.ContentLength = 0;
            return Ok();
        }

        // OPTIONS: api/Flats/5
        [HttpOptions("{id}")]
        public IActionResult OptionsFlats(int id)
        {
            Response.Headers.Add("Allow", "GET, PUT, PATCH, DELETE, HEAD, OPTIONS");
            return Ok();
        }
    }
}
