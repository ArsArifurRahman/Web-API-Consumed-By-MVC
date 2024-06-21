using Microsoft.AspNetCore.Mvc;
using Server.DTOs.Author;
using Server.Repository.Contracts;

namespace Server.Controllers;

[ApiController]
[Route("api/authors")]
public class AuthorController : ControllerBase
{
    private readonly IServerAuthorRepository _repository;

    public AuthorController(IServerAuthorRepository repository)
    {
        _repository = repository ?? throw new ArgumentNullException(nameof(repository));
    }

    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<IEnumerable<AuthorListDto>>> GetAuthors()
    {
        try
        {
            var authors = await _repository.GetAuthorsAsync();
            return Ok(authors);
        }
        catch (Exception ex)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, $"Error retrieving authors: {ex.Message}");
        }
    }

    [HttpGet("{authorId}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<AuthorReadDto>> GetAuthor(int authorId)
    {
        try
        {
            var author = await _repository.GetAuthorAsync(authorId);
            return Ok(author);
        }
        catch (KeyNotFoundException)
        {
            return NotFound();
        }
        catch (Exception ex)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, $"Error retrieving author with ID {authorId}: {ex.Message}");
        }
    }

    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<AuthorReadDto>> CreateAuthorAsync([FromBody] AuthorCreateDto authorCreateDto)
    {
        if (authorCreateDto == null)
        {
            return BadRequest("AuthorCreateDto cannot be null.");
        }

        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        try
        {
            var createdAuthor = await _repository.CreateAuthorAsync(authorCreateDto);
            return CreatedAtAction(nameof(GetAuthor), new { authorId = createdAuthor.Id }, createdAuthor);
        }
        catch (ArgumentException ex)
        {
            return BadRequest(ex.Message);
        }
        catch (Exception ex)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, $"Error creating author: {ex.Message}");
        }
    }

    [HttpPut("{authorId}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> UpdateAuthor(int authorId, [FromBody] AuthorUpdateDto authorUpdateDto)
    {
        if (authorUpdateDto == null)
        {
            return BadRequest("AuthorUpdateDto cannot be null.");
        }

        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        try
        {
            var updated = await _repository.UpdateAuthorAsync(authorId, authorUpdateDto);
            return updated ? NoContent() : NotFound();
        }
        catch (ArgumentException ex)
        {
            return BadRequest(ex.Message);
        }
        catch (KeyNotFoundException)
        {
            return NotFound();
        }
        catch (Exception ex)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, $"Error updating author with ID {authorId}: {ex.Message}");
        }
    }

    [HttpDelete("{authorId}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> DeleteAuthor(int authorId)
    {
        try
        {
            var deleted = await _repository.DeleteAuthorAsync(authorId);
            return deleted ? NoContent() : NotFound();
        }
        catch (Exception ex)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, $"Error deleting author with ID {authorId}: {ex.Message}");
        }
    }
}
