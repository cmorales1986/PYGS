using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PYGS.Api.Data;
using PYGS.Shared.Entities.AgriSQL;
using PYGS.Shared.Entities.HANASQL;

namespace PYGS.Api.Controllers.Adicionales
{
    [Route("api/adicional/[controller]")]
    [ApiController]
    public class GraficosProduccionController : ControllerBase
    {
        private readonly AgriSQLDbContext _context;

        public GraficosProduccionController(AgriSQLDbContext context)
        {
            _context = context;
        }

        [HttpGet("plantel")]
        public async Task<IActionResult> GetPlantel()
        {
            var items = await _context.graficoProduccionAgrinesses
                .Where(p => p.anhoo > 2023 && (p.categoria_num == 1 || p.categoria_num == 2))
                .OrderBy(p => p.categoria_num)
                .ThenBy(p => p.reportenum)
                .ThenByDescending(p => p.mesnum)
                .ToListAsync();
            return Ok(items);
        }

        [HttpGet("maternidad")]
        public async Task<IActionResult> GetMaternidad()
        {
            var items = await _context.graficoProduccionAgrinesses
                .Where(p => p.anhoo > 2023 && (p.categoria_num == 3))
                .OrderBy(p => p.categoria_num)
                .ThenBy(p => p.reportenum)
                .ThenByDescending(p => p.mesnum)
                .ToListAsync();
            return Ok(items);
        }
        [HttpGet("terminacion")]
        public async Task<IActionResult> GetTerminacion()
        {
            var items = await _context.graficoProduccionAgrinesses
                .Where(p => p.anhoo > 2023 && (p.categoria_num == 4))
                .OrderBy(p => p.categoria_num)
                .ThenBy(p => p.reportenum)
                .ThenByDescending(p => p.mesnum)
                .ToListAsync();
            return Ok(items);
        }



        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            var item = await _context.graficoProduccionAgrinesses
            .Where(p => p.id == id && p.anhoo > 2023)
            .FirstOrDefaultAsync();

            if (item == null)
            {
                return NotFound();
            }

            return Ok(item);
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] GraficoProduccionAgriness item)
        {
            _context.graficoProduccionAgrinesses.Add(item);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(Get), new { id = item.id }, item);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, [FromBody] GraficoProduccionAgriness item)
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
                if (!_context.graficoProduccionAgrinesses.Any(e => e.id == id))
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
            var item = await _context.graficoProduccionAgrinesses.FindAsync(id);
            if (item == null)
            {
                return NotFound();
            }

            _context.graficoProduccionAgrinesses.Remove(item);
            await _context.SaveChangesAsync();

            return NoContent();
        }

    }
}
