using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Server.DTOs;
using Server.DTOs.Author;
using Server.Repository.Contracts;

namespace Server.Controllers;

[Route("api/[controller]")]
[ApiController]
public class AuthorController : ControllerBase
{
    private readonly IAuthorRepository _repository;

    public AuthorController(IAuthorRepository repository)
    {
        _repository = repository;
    }

    // Get all authors
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetAuthors()
    {
        try
        {
            var authors = await _repository.GetAuthorsAsync();

            if (authors == null)
            {
                return NotFound("Authors not found");
            }

            return Ok(authors);
        }
        catch (Exception ex)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, $"Internal server error: {ex.Message}");
        }
    }

    // Get a specific author by Id
    [HttpGet("{authorId}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetAuthor(int authorId)
    {
        if (authorId <= 0)
        {
            return BadRequest("Author Id must be greater than zero");
        }

        try
        {
            var author = await _repository.GetAuthorAsync(authorId);

            if (author == null)
            {
                return NotFound($"Author with Id {authorId} not found");
            }

            return Ok(author);
        }
        catch (InvalidOperationException ex)
        {
            return NotFound(ex.Message);
        }
        catch (Exception ex)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, $"Internal server error: {ex.Message}");
        }
    }

    // Get books of a specific author by author Id
    [HttpGet("{authorId}/books")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetBooksOfAnAuthor(int authorId)
    {
        if (authorId <= 0)
        {
            return BadRequest("Author Id must be greater than zero");
        }

        try
        {
            var books = await _repository.GetBooksOfAnAuthorAsync(authorId);

            if (books == null || !books.Any())
            {
                return NotFound($"No books found for author with ID {authorId}");
            }

            return Ok(books);
        }
        catch (ArgumentException ex)
        {
            return BadRequest(ex.Message);
        }
        catch (Exception ex)
        {
            // Log the exception for internal server troubleshooting
            return StatusCode(StatusCodes.Status500InternalServerError, $"Internal server error: {ex.Message}");
        }
    }

    // Get authors of a specific book by book Id
    [HttpGet("book/{bookId}/authors")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetAuthorsOfABook(int bookId)
    {
        if (bookId <= 0)
        {
            return BadRequest("Book Id must be greater than zero");
        }

        try
        {
            var authors = await _repository.GetAuthorsOfABookAsync(bookId);

            if (authors == null || !authors.Any())
            {
                return NotFound($"No authors found for book with Id {bookId}");
            }

            return Ok(authors);
        }
        catch (ArgumentException ex)
        {
            return BadRequest(ex.Message);
        }
        catch (InvalidOperationException ex)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
        }
        catch (Exception ex)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, $"Unexpected error occurred: {ex.Message}");
        }
    }

    // Create a new author
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> PostAuthor(AuthorAddOrEditDto authorAddOrEditDto)
    {
        if (authorAddOrEditDto == null)
        {
            return BadRequest("Author DTO cannot be null");
        }

        try
        {
            var createdAuthor = await _repository.CreateAuthorAsync(authorAddOrEditDto);

            var resultDto = new AuthorDto
            {
                Id = createdAuthor.Id,
                FirstName = createdAuthor.FirstName,
                LastName = createdAuthor.LastName,
                CountryId = createdAuthor.CountryId,
                CountryName = createdAuthor.CountryName
            };

            return CreatedAtAction(nameof(GetAuthor), new { authorId = resultDto.Id }, resultDto);
        }
        catch (ArgumentException ex)
        {
            return BadRequest(ex.Message);
        }
        catch (InvalidOperationException ex)
        {
            return Conflict(ex.Message);
        }
        catch (Exception ex)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, $"Internal server error: {ex.Message}");
        }
    }

    // Update an existing author
    [HttpPut("{authorId}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> PutAuthor(int authorId, AuthorAddOrEditDto authorAddOrEditDto)
    {
        if (authorId <= 0)
        {
            return BadRequest("Author Id must be greater than zero");
        }

        if (authorAddOrEditDto == null)
        {
            return BadRequest("Author DTO cannot be null");
        }

        try
        {
            await _repository.UpdateAuthorAsync(authorId, authorAddOrEditDto);
            return NoContent();
        }
        catch (ArgumentException ex)
        {
            return BadRequest(ex.Message);
        }
        catch (InvalidOperationException ex)
        {
            return Conflict(ex.Message);
        }
        catch (DbUpdateException ex)
        {
            // Handle specific database update exception if needed
            return StatusCode(StatusCodes.Status500InternalServerError, $"Database update error: {ex.Message}");
        }
        catch (Exception ex)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, $"Internal server error: {ex.Message}");
        }
    }

    // Delete an author
    [HttpDelete("{authorId}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> DeleteAuthor(int authorId)
    {
        if (authorId <= 0)
        {
            return BadRequest("Author Id must be greater than zero");
        }

        try
        {
            await _repository.DeleteAuthorAsync(authorId);
            return NoContent();
        }
        catch (ArgumentException ex)
        {
            return BadRequest(ex.Message);
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(ex.Message);
        }
        catch (Exception ex)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, $"Internal server error: {ex.Message}");
        }
    }
}
