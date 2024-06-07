using Microsoft.AspNetCore.Mvc;
using Server.DTOs.Author;
using Server.Models;
using Server.Repository.Contracts;

namespace Server.Controllers;

[Route("api/[controller]")]
[ApiController]
public class AuthorController : ControllerBase
{
    private readonly IAuthorRepository _authorRepository;

    public AuthorController(IAuthorRepository authorRepository)
    {
        _authorRepository = authorRepository;
    }

    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult<IEnumerable<AuthorListDto>>> GetAuthors()
    {
        var authors = await _authorRepository.GetAuthorsAsync();

        var authorListDto = authors.Select(author => new AuthorListDto
        {
            FirstName = author.FirstName ?? string.Empty,
            LastName = author.LastName ?? string.Empty,
        });

        return Ok(authorListDto);
    }

    [HttpGet("{authorId}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<AuthorReadDto>> GetAuthor(int authorId)
    {
        var author = await _authorRepository.GetAuthorAsync(authorId);

        if (author == null)
        {
            return NotFound("Author not found.");
        }

        var authorReadDto = new AuthorReadDto
        {
            Id = author.Id,
            FirstName = author.FirstName ?? string.Empty,
            LastName = author.LastName ?? string.Empty
        };

        return Ok(authorReadDto);
    }

    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    public async Task<ActionResult<Author>> PostAuthor(AuthorCreateDto authorCreateDto)
    {
        var author = new Author
        {
            FirstName = authorCreateDto.FirstName ?? string.Empty,
            LastName = authorCreateDto.LastName ?? string.Empty,
            CountryId = authorCreateDto.CountryId
        };

        var createdAuthor = await _authorRepository.AddAuthorAsync(author);
        return CreatedAtAction(nameof(GetAuthor), new { authorId = createdAuthor.Id }, createdAuthor);
    }

    [HttpPut("{authorId}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> PutAuthor(int authorId, AuthorUpdateDto authorUpdateDto)
    {
        if (authorId != authorUpdateDto.Id)
        {
            return BadRequest("Author ID mismatch.");
        }

        var author = await _authorRepository.GetAuthorAsync(authorId);

        if (author == null)
        {
            return NotFound("Author not found.");
        }

        author.FirstName = authorUpdateDto.FirstName ?? string.Empty;
        author.LastName = authorUpdateDto.LastName ?? string.Empty;
        await _authorRepository.EditAuthorAsync(authorId, author);

        return NoContent();
    }

    [HttpDelete("{authorId}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> DeleteAuthor(int authorId)
    {
        try
        {
            await _authorRepository.DeleteAuthorAsync(authorId);
        }
        catch (KeyNotFoundException)
        {
            return NotFound();
        }

        return NoContent();
    }
}
