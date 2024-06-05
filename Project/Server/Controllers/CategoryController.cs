using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Server.Data;
using Server.Models;

namespace Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoryController : ControllerBase
    {
        private readonly DataContext _context;

        public CategoryController(DataContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Category>>> GetCatagories()
        {
            return await _context.Catagories.ToListAsync();
        }

        [HttpGet("{categoryId}")]
        public async Task<ActionResult<Category>> GetCategory(int categoryId)
        {
            var category = await _context.Catagories.FindAsync(categoryId);

            if (category == null)
            {
                return NotFound();
            }

            return category;
        }

        [HttpPost]
        public async Task<ActionResult<Category>> PostCategory(Category category)
        {
            _context.Catagories.Add(category);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetCategory), new { id = category.Id }, category);
        }

        [HttpPut("{categoryId}")]
        public async Task<IActionResult> PutCategory(int categoryId, Category category)
        {
            if (categoryId != category.Id)
            {
                return BadRequest();
            }

            _context.Entry(category).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CategoryExists(categoryId))
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

        [HttpDelete("{categoryId}")]
        public async Task<IActionResult> DeleteCategory(int categoryId)
        {
            var category = await _context.Catagories.FindAsync(categoryId);
            if (category == null)
            {
                return NotFound();
            }

            _context.Catagories.Remove(category);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
