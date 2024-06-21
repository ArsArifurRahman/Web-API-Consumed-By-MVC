using Microsoft.AspNetCore.Mvc;
using Server.DTOs.Book;
using Server.Repository.Contracts;

namespace Server.Controllers;

[ApiController]
[Route("api/[controller]")]
public class BookController : ControllerBase
{
    private readonly IServerBookRepository _repository;

    public BookController(IServerBookRepository repository)
    {
        _repository = repository ?? throw new ArgumentNullException(nameof(repository));
    }

    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> GetBooks()
    {
        return Ok(await _repository.GetBooksAsync());
    }

    [HttpGet("{bookId}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetBookById(int bookId)
    {
        try
        {
            return Ok(await _repository.GetBookByIdAsync(bookId));
        }
        catch (KeyNotFoundException)
        {
            return NotFound();
        }
    }

    [HttpGet("isbn/{bookIsbn}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetBookByIsbn(string bookIsbn)
    {
        try
        {
            return Ok(await _repository.GetBookByIsbnAsync(bookIsbn));
        }
        catch (KeyNotFoundException)
        {
            return NotFound();
        }
    }

    [HttpGet("{bookId}/rating")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetBookRating(int bookId)
    {
        try
        {
            return Ok(await _repository.GetBookByRating(bookId));
        }
        catch (KeyNotFoundException)
        {
            return NotFound();
        }
    }

    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> AddBook([FromBody] BookCreateDto bookCreateDto, [FromQuery] List<int> authorIds, [FromQuery] List<int> categoryIds)
    {
        if (bookCreateDto == null)
        {
            return BadRequest("BookCreateDto cannot be null.");
        }

        try
        {
            var bookReadDto = await _repository.AddBookAsync(authorIds, categoryIds, bookCreateDto);
            return CreatedAtAction(nameof(GetBookById), new { bookId = bookReadDto.Id }, bookReadDto);
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpPut("{bookId}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> EditBook(int bookId, [FromBody] BookUpdateDto bookUpdateDto, [FromQuery] List<int> authorIds, [FromQuery] List<int> categoryIds)
    {
        if (bookUpdateDto == null)
        {
            return BadRequest("BookUpdateDto cannot be null.");
        }

        try
        {
            var result = await _repository.EditBookAsync(authorIds, categoryIds, bookId, bookUpdateDto);
            return result ? NoContent() : BadRequest("Failed to update the book.");
        }
        catch (KeyNotFoundException)
        {
            return NotFound();
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpDelete("{bookId}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> DeleteBook(int bookId)
    {
        try
        {
            var result = await _repository.DeleteBookAsync(bookId);
            return result ? NoContent() : BadRequest("Failed to delete the book.");
        }
        catch (KeyNotFoundException)
        {
            return NotFound();
        }
    }
}
