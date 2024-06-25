using Microsoft.EntityFrameworkCore;
using Server.Data;
using Server.DTOs;
using Server.DTOs.Author;
using Server.DTOs.Book;
using Server.Models;
using Server.Repository.Contracts;

namespace Server.Repository.Implementations;

public class AuthorRepository : IAuthorRepository
{
    private readonly DataContext _context;

    public AuthorRepository(DataContext context)
    {
        _context = context;
    }

    // Create
    public async Task<AuthorDto> CreateAuthorAsync(AuthorAddOrEditDto authorCreateDto)
    {
        if (authorCreateDto == null)
            throw new ArgumentNullException(nameof(authorCreateDto));

        if (string.IsNullOrWhiteSpace(authorCreateDto.FirstName))
            throw new ArgumentException("First name is required.", nameof(authorCreateDto.FirstName));

        if (string.IsNullOrWhiteSpace(authorCreateDto.LastName))
            throw new ArgumentException("Last name is required.", nameof(authorCreateDto.LastName));

        if (authorCreateDto.CountryId <= 0)
            throw new ArgumentException("Country ID must be a positive integer.", nameof(authorCreateDto.CountryId));

        var existingAuthor = await _context.Authors
            .FirstOrDefaultAsync(a => a.FirstName == authorCreateDto.FirstName && a.LastName == authorCreateDto.LastName);

        if (existingAuthor != null)
            throw new InvalidOperationException("An author with the same first and last name already exists.");

        var country = await _context.Countries.FindAsync(authorCreateDto.CountryId);
        if (country == null)
            throw new InvalidOperationException("The specified country does not exist.");

        var author = new Author
        {
            FirstName = authorCreateDto.FirstName,
            LastName = authorCreateDto.LastName,
            CountryId = authorCreateDto.CountryId
        };

        _context.Authors.Add(author);
        await _context.SaveChangesAsync();

        return new AuthorDto
        {
            Id = author.Id,
            FirstName = author.FirstName,
            LastName = author.LastName,
            CountryId = author.CountryId,
            CountryName = country.Name
        };
    }

    // Read
    public async Task<AuthorDto> GetAuthorAsync(int authorId)
    {
        if (authorId <= 0)
        {
            throw new ArgumentException("Author ID must be greater than zero.", nameof(authorId));
        }

        var author = await _context.Authors
            .Include(a => a.Country)
            .FirstOrDefaultAsync(a => a.Id == authorId);

        if (author == null)
        {
            throw new KeyNotFoundException($"Author with ID {authorId} not found.");
        }

        return new AuthorDto
        {
            Id = author.Id,
            FirstName = author.FirstName,
            LastName = author.LastName,
            CountryId = author.CountryId,
            CountryName = author.Country!.Name
        };
    }

    public async Task<IEnumerable<AuthorReadDto>> GetAuthorsAsync()
    {
        try
        {
            var authors = await _context.Authors
                .Include(a => a.Country)
                .ToListAsync();

            if (authors == null || !authors.Any())
            {
                throw new KeyNotFoundException("No authors found in the database.");
            }

            return authors.Select(author => new AuthorReadDto
            {
                FirstName = author.FirstName,
                LastName = author.LastName,
                CountryName = author.Country!.Name
            }).ToList();
        }
        catch (Exception ex)
        {
            throw new ApplicationException("An error occurred while fetching authors.", ex);
        }
    }

    public async Task<IEnumerable<AuthorReadDto>> GetAuthorsOfABookAsync(int bookId)
    {
        try
        {
            var bookExists = await _context.Books.AnyAsync(b => b.Id == bookId);
            if (!bookExists)
            {
                throw new ArgumentException($"Book with ID {bookId} does not exist.");
            }

            var authors = await _context.BookAuthors
                .Where(ba => ba.BookId == bookId)
                .Select(ba => new AuthorReadDto
                {
                    FirstName = ba.Author!.FirstName,
                    LastName = ba.Author.LastName,
                    CountryName = ba.Author.Country!.Name
                })
                .ToListAsync();

            if (authors == null || !authors.Any())
            {
                throw new InvalidOperationException($"No authors found for Book with ID {bookId}.");
            }

            return authors;
        }
        catch (Exception ex)
        {
            throw new Exception("Error retrieving authors of a book. See inner exception for details.", ex);
        }
    }

    public async Task<IEnumerable<BookReadDto>> GetBooksOfAnAuthorAsync(int authorId)
    {
        if (authorId <= 0)
        {
            throw new ArgumentException("Author ID must be greater than zero.", nameof(authorId));
        }

        try
        {
            var books = await _context.BookAuthors
                .Where(ba => ba.AuthorId == authorId)
                .Select(ba => ba.Book)
                .ToListAsync();

            if (books == null || !books.Any())
            {
                throw new Exception($"No books found for author with ID {authorId}.");
            }

            return books.Select(b => new BookReadDto
            {
                Isbn = b!.Isbn,
                Title = b.Title,
                PublishedAt = b.PublishedAt
            }).ToList();
        }
        catch (Exception ex)
        {
            throw new Exception("Error retrieving books of an author.", ex);
        }
    }

    // Update
    public async Task<bool> UpdateAuthorAsync(int authorId, AuthorAddOrEditDto authorAddOrEditDto)
    {
        var author = await _context.Authors.FirstOrDefaultAsync(a => a.Id == authorId);

        if (author == null)
        {
            throw new ArgumentException($"Author with ID {authorId} not found.");
        }

        if (await _context.Authors.AnyAsync(a =>
            a.Id != authorId &&
            a.FirstName == authorAddOrEditDto.FirstName &&
            a.LastName == authorAddOrEditDto.LastName))
        {
            throw new InvalidOperationException($"Author with the name '{authorAddOrEditDto.FirstName} {authorAddOrEditDto.LastName}' already exists.");
        }

        var country = await _context.Countries.FindAsync(authorAddOrEditDto.CountryId);
        if (country == null)
        {
            throw new ArgumentException($"Country with ID {authorAddOrEditDto.CountryId} not found.");
        }

        author.FirstName = authorAddOrEditDto.FirstName;
        author.LastName = authorAddOrEditDto.LastName;
        author.CountryId = authorAddOrEditDto.CountryId;

        try
        {
            await _context.SaveChangesAsync();
            return true;
        }
        catch (DbUpdateException ex)
        {
            throw new Exception("Failed to update author in the database. See inner exception for details.", ex);
        }
    }

    // Delete
    public async Task<bool> DeleteAuthorAsync(int authorId)
    {
        if (authorId <= 0)
        {
            throw new ArgumentException("Invalid author ID");
        }

        try
        {
            var author = await _context.Authors.FindAsync(authorId);
            if (author == null)
            {
                return false;
            }

            _context.Authors.Remove(author);
            await _context.SaveChangesAsync();
            return true;
        }
        catch (DbUpdateException dbEx)
        {
            throw new Exception("An error occurred while updating the database", dbEx);
        }
        catch (Exception ex)
        {
            throw new Exception("An unexpected error occurred", ex);
        }
    }
}
