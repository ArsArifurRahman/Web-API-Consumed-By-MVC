using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Server.Data;
using Server.Models;

namespace Server.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ReviewerController : ControllerBase
{
    private readonly DataContext _context;

    public ReviewerController(DataContext context)
    {
        _context = context;
    }

    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult<IEnumerable<Reviewer>>> GetReviewers()
    {
        return await _context.Reviewers.ToListAsync();
    }

    [HttpGet("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<Reviewer>> GetReviewer(int id)
    {
        var reviewer = await _context.Reviewers.FindAsync(id);

        if (reviewer == null)
        {
            return NotFound();
        }

        return reviewer;
    }

    [HttpPut("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> PutReviewer(int id, Reviewer reviewer)
    {
        if (id != reviewer.Id)
        {
            return BadRequest();
        }

        _context.Entry(reviewer).State = EntityState.Modified;

        try
        {
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!ReviewerExists(id))
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

    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    public async Task<ActionResult<Reviewer>> PostReviewer(Reviewer reviewer)
    {
        _context.Reviewers.Add(reviewer);
        await _context.SaveChangesAsync();

        return CreatedAtAction("GetReviewer", new { id = reviewer.Id }, reviewer);
    }

    [HttpDelete("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> DeleteReviewer(int id)
    {
        var reviewer = await _context.Reviewers.FindAsync(id);
        if (reviewer == null)
        {
            return NotFound();
        }

        _context.Reviewers.Remove(reviewer);
        await _context.SaveChangesAsync();

        return NoContent();
    }

    private bool ReviewerExists(int id)
    {
        return _context.Reviewers.Any(e => e.Id == id);
    }
}

