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
    public class DistrictsController : ControllerBase
    {
        private readonly RentAPIContext _context;

        public DistrictsController(RentAPIContext context)
        {
            _context = context;
        }

        // GET: api/Districts
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Districts>>> GetDistricts()
        {
            return await _context.Districts.ToListAsync();
        }

        // GET: api/Districts/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Districts>> GetDistricts(int id)
        {
            var districts = await _context.Districts.FindAsync(id);

            if (districts == null)
            {
                return NotFound();
            }

            return districts;
        }

        // PUT: api/Districts/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutDistricts(int id, Districts districts)
        {
            if (id != districts.Id)
            {
                return BadRequest();
            }

            _context.Entry(districts).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!DistrictsExists(id))
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

        // POST: api/Districts
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Districts>> PostDistricts(Districts districts)
        {
            _context.Districts.Add(districts);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetDistricts", new { id = districts.Id }, districts);
        }

        // DELETE: api/Districts/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteDistricts(int id)
        {
            var districts = await _context.Districts.FindAsync(id);
            if (districts == null)
            {
                return NotFound();
            }

            _context.Districts.Remove(districts);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool DistrictsExists(int id)
        {
            return _context.Districts.Any(e => e.Id == id);
        }

        [HttpPatch("{id}")]
        public async Task<IActionResult> PatchDistrict(int id, [FromBody] JsonElement updates)
        {
            var district = await _context.Districts.FindAsync(id);
            if (district == null)
                return NotFound();

            var json = updates.GetRawText();
            var patchDoc = JsonSerializer.Deserialize<Dictionary<string, JsonElement>>(json);

            foreach (var entry in patchDoc)
            {
                var property = typeof(Districts).GetProperty(entry.Key);
                if (property != null)
                {
                    var value = Convert.ChangeType(entry.Value.ToString(), property.PropertyType);
                    property.SetValue(district, value);
                }
            }

            await _context.SaveChangesAsync();
            return NoContent();
        }

        [HttpHead("{id}")]
        public async Task<IActionResult> HeadDistrict(int id)
        {
            var exists = await _context.Districts.AnyAsync(d => d.Id == id);
            if (!exists)
                return NotFound();

            return Ok();
        }

        [HttpOptions("{id}")]
        public IActionResult OptionsDistrict(int id)
        {
            Response.Headers.Add("Allow", "GET, PUT, PATCH, DELETE, HEAD, OPTIONS");
            return Ok();
        }

    }
}