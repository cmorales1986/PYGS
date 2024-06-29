using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PYGS.Api.Data;
using PYGS.Shared.Entities;

namespace PYGS.Api.Controllers.Adicionales
{
    [Route("api/adicional/[controller]")]
    [ApiController]
    public class ProgramacionCargaController: ControllerBase
    {
        private readonly HanaDbContext _context;

        public ProgramacionCargaController(HanaDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            return Ok(await _context.ProgramacionCargaCerdos.ToListAsync());
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            var item = await _context.ProgramacionCargaCerdos.FindAsync(id);
            if (item == null)
            {
                return NotFound();
            }
            return Ok(item);
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] ProgramacionCargaCerdos item)
        {
            _context.ProgramacionCargaCerdos.Add(item);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(Get), new { id = item.Id }, item);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, [FromBody] ProgramacionCargaCerdos item)
        {
            if (id != item.Id)
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
                if (!_context.ProgramacionCargaCerdos.Any(e => e.Id == id))
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
            var item = await _context.ProgramacionCargaCerdos.FindAsync(id);
            if (item == null)
            {
                return NotFound();
            }

            _context.ProgramacionCargaCerdos.Remove(item);
            await _context.SaveChangesAsync();

            return NoContent();
        }


    }
}
