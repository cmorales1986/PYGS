using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PYGS.Api.Data;
using PYGS.Shared.Entities;

namespace PYGS.Api.Controllers
{
    [ApiController]
    //[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [Route("/api/categories")]
    public class CategoriesController : ControllerBase
    {
        private readonly DataContext _context;

        public CategoriesController(DataContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> GetAsync()
        {
            return Ok(await _context.Categorias.ToListAsync());
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetAsync(int id)
        {
            var category = await _context.Categorias.FirstOrDefaultAsync(x => x.Id == id);
            if (category == null)
            {
                return NotFound();
            }

            return Ok(category);

        }

        [HttpPost]
        public async Task<ActionResult> PostAsync(Categoria categoria)
        {
            try
            {
                categoria.FechaAlta = DateTime.Now;
                categoria.UserAlta = HttpContext.User.Identity?.Name;

                _context.Add(categoria);
                await _context.SaveChangesAsync();
                return Ok(categoria);
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
        public async Task<ActionResult> PutAsync(Categoria categoria)
        {
            try
            {
                categoria.FechaModi = DateTime.Now;
                categoria.UserModi = HttpContext.User.Identity?.Name;

                _context.Update(categoria);
                await _context.SaveChangesAsync();
                return Ok(categoria);
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
            var category = await _context.Categorias.FirstOrDefaultAsync(x => x.Id == id);
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
