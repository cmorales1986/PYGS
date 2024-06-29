using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PYGS.Api.Data;
using PYGS.Shared.Entities;
using PYGS.Shared.Entities.HANASQL;

namespace PYGS.Api.Controllers.Adicionales
{
    [Route("api/adicional/[controller]")]
    [ApiController]
    public class ProgramacionMensualController : ControllerBase
    {
        private readonly HanaDbContext _context;

        public ProgramacionMensualController(HanaDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var items = await _context.programacionCerdoMensuals
                .Where(p => p.fecha_ini.Year > 2023)
                .OrderByDescending(p => p.id)
                .ToListAsync();
            return Ok(items);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            var item = await _context.programacionCerdoMensuals
            .Where(p => p.id == id && p.fecha_ini.Year > 2023)
            .FirstOrDefaultAsync();

            if (item == null)
            {
                return NotFound();
            }

            return Ok(item);
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] ProgramacionCerdoMensual item)
        {
            _context.programacionCerdoMensuals.Add(item);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(Get), new { id = item.id }, item);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, [FromBody] ProgramacionCerdoMensual item)
        {
            if (id != item.id)
            {
                return BadRequest();
            }

            _context.Entry(item).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!_context.programacionCerdoMensuals.Any(e => e.id == id))
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

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var item = await _context.programacionCerdoMensuals.FindAsync(id);
            if (item == null)
            {
                return NotFound();
            }

            _context.programacionCerdoMensuals.Remove(item);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
