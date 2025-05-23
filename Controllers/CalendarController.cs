using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using CalendarService.Data;
using CalendarService.Models;

// Tips  & Trix video och felsökt med chatgpt

namespace CalendarService.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CalendarController : ControllerBase
    {
        private readonly CalendarDbContext _db;
        public CalendarController(CalendarDbContext db) => _db = db;

        
        [HttpGet]
        public async Task<IEnumerable<CalendarEvent>> Get(int year, int month)
            => await _db.CalendarEvents
                        .Where(e => e.Date.Year == year && e.Date.Month == month)
                        .ToListAsync();

       
        [HttpPost]
        public async Task<ActionResult<CalendarEvent>> Post(CalendarEvent ev)
        {
            _db.CalendarEvents.Add(ev);
            await _db.SaveChangesAsync();
            return CreatedAtAction(nameof(GetById), new { id = ev.Id }, ev);
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult<CalendarEvent>> GetById(int id)
        {
            var ev = await _db.CalendarEvents.FindAsync(id);
            if (ev == null) return NotFound();
            return ev;
        }

       
        [HttpPut("{id:int}")]
        public async Task<IActionResult> Put(int id, CalendarEvent ev)
        {
            if (id != ev.Id) return BadRequest("Id mismatch");
            _db.Entry(ev).State = EntityState.Modified;
            try
            {
                await _db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!await _db.CalendarEvents.AnyAsync(e => e.Id == id))
                    return NotFound();
                throw;
            }
            return NoContent();
        }

      
        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            var ev = await _db.CalendarEvents.FindAsync(id);
            if (ev == null) return NotFound();
            _db.CalendarEvents.Remove(ev);
            await _db.SaveChangesAsync();
            return NoContent();
        }
    }
}
