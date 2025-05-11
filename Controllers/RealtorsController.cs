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
    public class RealtorsController : ControllerBase
    {
        private readonly RentAPIContext _context;

        public RealtorsController(RentAPIContext context)
        {
            _context = context;
        }

        // GET: api/Realtors
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Realtors>>> GetRealtors()
        {
            return await _context.Realtors.ToListAsync();
        }

        // GET: api/Realtors/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Realtors>> GetRealtors(int id)
        {
            var realtors = await _context.Realtors.FindAsync(id);

            if (realtors == null)
            {
                return NotFound();
            }

            return realtors;
        }

        // PUT: api/Realtors/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutRealtors(int id, Realtors realtors)
        {
            if (id != realtors.Id)
            {
                return BadRequest();
            }

            if (_context.Realtors.Any(r => r.RlEmail == realtors.RlEmail && r.Id != id))
            {
                return BadRequest("Електронна пошта вже використовується");
            }

            if (_context.Realtors.Any(r => r.RlPhone == realtors.RlPhone && r.Id != id))
            {
                return BadRequest("Номер телефону вже використовується");
            }

            _context.Entry(realtors).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!RealtorsExists(id))
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

        // POST: api/Realtors
        [HttpPost]
        public async Task<ActionResult<Realtors>> PostRealtors(Realtors realtors)
        {
            if (_context.Realtors.Any(r => r.RlEmail == realtors.RlEmail))
            {
                return BadRequest("Електронна пошта вже використовується");
            }

            if (_context.Realtors.Any(r => r.RlPhone == realtors.RlPhone))
            {
                return BadRequest("Номер телефону вже використовується");
            }

            _context.Realtors.Add(realtors);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetRealtors", new { id = realtors.Id }, realtors);
        }

        // DELETE: api/Realtors/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteRealtors(int id)
        {
            var realtors = await _context.Realtors.FindAsync(id);
            if (realtors == null)
            {
                return NotFound();
            }

            _context.Realtors.Remove(realtors);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool RealtorsExists(int id)
        {
            return _context.Realtors.Any(e => e.Id == id);
        }

        // PATCH: api/Realtors/5
        [HttpPatch("{id}")]
        public async Task<IActionResult> PatchRealtors(int id, [FromBody] JsonElement updates)
        {
            var realtor = await _context.Realtors.FindAsync(id);
            if (realtor == null)
            {
                return NotFound();
            }

            var patchDoc = JsonSerializer.Deserialize<Dictionary<string, JsonElement>>(updates.GetRawText());

            foreach (var entry in patchDoc)
            {
                var property = typeof(Realtors).GetProperty(entry.Key);
                if (property != null && property.CanWrite)
                {
                    try
                    {
                        object value = entry.Value.ValueKind switch
                        {
                            JsonValueKind.String => entry.Value.GetString(),
                            JsonValueKind.Number when property.PropertyType == typeof(int) => entry.Value.GetInt32(),
                            JsonValueKind.Number when property.PropertyType == typeof(double) => entry.Value.GetDouble(),
                            JsonValueKind.Number when property.PropertyType == typeof(decimal) => entry.Value.GetDecimal(),
                            JsonValueKind.True => true,
                            JsonValueKind.False => false,
                            _ => null
                        };

                        if (value != null)
                        {
                            property.SetValue(realtor, value);
                        }
                    }
                    catch (Exception ex)
                    {
                        return BadRequest($"Помилка при оновленні поля '{entry.Key}': {ex.Message}");
                    }
                }
            }

            if (_context.Realtors.Any(r => r.RlEmail == realtor.RlEmail && r.Id != id))
            {
                return BadRequest("Електронна пошта вже використовується");
            }

            if (_context.Realtors.Any(r => r.RlPhone == realtor.RlPhone && r.Id != id))
            {
                return BadRequest("Номер телефону вже використовується");
            }

            await _context.SaveChangesAsync();
            return NoContent();
        }

        // HEAD: api/Realtors/5
        [HttpHead("{id}")]
        public async Task<IActionResult> HeadRealtors(int id)
        {
            var exists = await _context.Realtors.AnyAsync(r => r.Id == id);

            if (!exists)
            {
                return NotFound();
            }

            Response.ContentLength = 0; 
            return Ok(); 
        }

        // OPTIONS: api/Realtors/5
        [HttpOptions("{id}")]
        public IActionResult OptionsRealtors(int id)
        {
            Response.Headers.Add("Allow", "GET, PUT, PATCH, DELETE, HEAD, OPTIONS");
            return Ok();
        }
    }
}
