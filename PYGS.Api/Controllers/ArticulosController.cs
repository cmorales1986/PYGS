using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PYGS.Api.Data;
using PYGS.Shared.Entities;

namespace PYGS.Api.Controllers
{
    [ApiController]
    //[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [Route("/api/articulos")]
    public class ArticulosController : ControllerBase
    {
        private readonly DataContext _context;

        public ArticulosController(DataContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> GetAsync()
        {
            return Ok(await _context.Articulos.ToListAsync());
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetAsync(int id)
        {
            var category = await _context.Articulos.FirstOrDefaultAsync(x => x.Id == id);
            if (category == null)
            {
                return NotFound();
            }

            return Ok(category);

        }

        [HttpPost]
        public async Task<ActionResult> PostAsync(Articulo articulos)
        {
            try
            {
                //articulos.FechaAlta = DateTime.Now;
                //articulos.UserAlta = HttpContext.User.Identity?.Name;

                _context.Add(articulos);
                await _context.SaveChangesAsync();
                return Ok(articulos);
            }
            catch (DbUpdateException dbUpdateException)
            {

                if (dbUpdateException.InnerException!.Message.Contains("duplicate"))
                {
                    return BadRequest("Ya existe una cateogoria con el mismo nombre.");
                }

                return BadRequest(dbUpdateException.Message);
            }
            catch (Exception exception)
            {
                return BadRequest(exception.Message);
            }
        }

        [HttpPut]
        public async Task<ActionResult> PutAsync(Articulo articulo)
        {
            try
            {
                //categoria.FechaModi = DateTime.Now;
                //categoria.UserModi = HttpContext.User.Identity?.Name;

                _context.Update(articulo);
                await _context.SaveChangesAsync();
                return Ok(articulo);
            }
            catch (DbUpdateException dbUpdateException)
            {

                if (dbUpdateException.InnerException!.Message.Contains("duplicate"))
                {
                    return BadRequest("Ya existe una cateogoria con el mismo nombre.");
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
            var category = await _context.Articulos.FirstOrDefaultAsync(x => x.Id == id);
            if (category == null)
            {
                return NotFound();
            }

            _context.Remove(category);
            await _context.SaveChangesAsync();
            return NoContent();

        }
    }
}
