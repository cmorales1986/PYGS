using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PYGS.Api.Data;
using PYGS.Shared.Entities;

namespace PYGS.Api.Controllers
{
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [Route("/api/ciudades")]
    public class CiudadesController : ControllerBase
    {
        private readonly DataContext _context;

        public CiudadesController(DataContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> GetAsync()
        {
            return Ok(await _context.Ciudades.ToListAsync());
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetAsync(int id)
        {
            var ciudad = await _context.Ciudades.FirstOrDefaultAsync(x => x.Id == id);
            if (ciudad == null)
            {
                return NotFound();
            }

            return Ok(ciudad);

        }

        [HttpGet("combo")]
        public async Task<ActionResult> GetCombo()
        {
            return Ok(await _context.Ciudades.ToListAsync());
        }

        [HttpPost]
        public async Task<ActionResult> PostAsync(Ciudad ciudad)
        {
            try
            {
                _context.Add(ciudad);
                await _context.SaveChangesAsync();
                return Ok(ciudad);
            }
            catch (DbUpdateException dbUpdateException)
            {

                if (dbUpdateException.InnerException!.Message.Contains("duplicate"))
                {
                    return BadRequest("Ya existe una ciudad con el mismo nombre.");
                }

                return BadRequest(dbUpdateException.Message);
            }
            catch (Exception exception)
            {
                return BadRequest(exception.Message);
            }
        }

        [HttpPut]
        public async Task<ActionResult> PutAsync(Ciudad ciudad)
        {
            try
            {

                _context.Update(ciudad);
                await _context.SaveChangesAsync();
                return Ok(ciudad);
            }
            catch (DbUpdateException dbUpdateException)
            {

                if (dbUpdateException.InnerException!.Message.Contains("duplicate"))
                {
                    return BadRequest("Ya existe una ciudad con el mismo nombre.");
                }

                return BadRequest(dbUpdateException.Message);
            }
            catch (Exception exception)
            {
                return BadRequest(exception.Message);
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAsync(int id)
        {
            var ciudad = await _context.Ciudades.FirstOrDefaultAsync(x => x.Id == id);
            if (ciudad == null)
            {
                return NotFound();
            }

            _context.Remove(ciudad);
            await _context.SaveChangesAsync();
            return NoContent();

        }

    }
}
