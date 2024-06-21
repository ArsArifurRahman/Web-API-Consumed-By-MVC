using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.EntityFrameworkCore;
using Server.Data;
using Server.DTOs.Book;
using Server.Models;
using Server.Repository.Contracts;
using System.Linq.Expressions;

namespace Server.Repository.Services;

public class ServerBookRepository : IServerBookRepository
{
    private readonly DataContext _context;

    public ServerBookRepository(DataContext context)
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));
    }

    public async Task<IEnumerable<BookListDto>> GetBooksAsync()
    {
        var books = await _context.Books.AsNoTracking().ToListAsync();

        return books.Select(book => new BookListDto
        {
            Title = book.Title,
            Isbn = book.Isbn,
            PublishedAt = book.PublishedAt
        }).ToList();
    }

    public async Task<BookReadDto> GetBookByIdAsync(int bookId)
    {
        var book = await GetBookIncludingDetailsAsync(b => b.Id == bookId);

        if (book == null)
        {
            throw new KeyNotFoundException("Book not found.");
        }

        return MapToBookReadDto(book);
    }

    public async Task<BookReadDto> GetBookByIsbnAsync(string bookIsbn)
    {
        var book = await GetBookIncludingDetailsAsync(b => b.Isbn == bookIsbn);

        if (book == null)
        {
            throw new KeyNotFoundException("Book not found.");
        }

        return MapToBookReadDto(book);
    }

    public async Task<decimal> GetBookByRating(int bookId)
    {
        var book = await _context.Books.AsNoTracking()
            .Include(b => b.Reviews)
            .FirstOrDefaultAsync(b => b.Id == bookId);

        if (book == null)
        {
            throw new KeyNotFoundException("Book not found.");
        }

        var ratings = book.Reviews.Select(r => r.Rating);
        return ratings.Any() ? (decimal)ratings.Average() : 0;
    }

    public async Task<BookReadDto> AddBookAsync(List<int> authorIds, List<int> categoryIds, BookCreateDto bookCreateDto)
    {
        if (bookCreateDto == null)
        {
            throw new ArgumentNullException(nameof(bookCreateDto), "BookCreateDto cannot be null.");
        }

        var bookReadDto = new BookReadDto
        {
            Title = bookCreateDto.Title,
            Isbn = bookCreateDto.Isbn,
            PublishedAt = bookCreateDto.PublishedAt
        };

        var isValid = await ValidateBookAsync(authorIds, categoryIds, bookReadDto);
        if (!isValid)
        {
            throw new InvalidOperationException("Book validation failed.");
        }

        var book = new Book
        {
            Title = bookCreateDto.Title,
            Isbn = bookCreateDto.Isbn,
            PublishedAt = bookCreateDto.PublishedAt
        };

        book.BookAuthors = authorIds.Select(id => new BookAuthor { AuthorId = id, Book = book }).ToList();
        book.BookCategories = categoryIds.Select(id => new BookCategory { CategoryId = id, Book = book }).ToList();

        await _context.Books.AddAsync(book);
        await _context.SaveChangesAsync();

        return new BookReadDto
        {
            Id = book.Id,
            Title = book.Title,
            Isbn = book.Isbn,
            PublishedAt = book.PublishedAt
        };
    }

    public async Task<bool> EditBookAsync(List<int> authorIds, List<int> categoryIds, int bookId, BookUpdateDto bookUpdateDto)
    {
        if (bookUpdateDto == null)
        {
            throw new ArgumentNullException(nameof(bookUpdateDto), "BookUpdateDto cannot be null.");
        }

        var existingBook = await _context.Books
            .Include(b => b.BookAuthors)
            .Include(b => b.BookCategories)
            .FirstOrDefaultAsync(b => b.Id == bookId);
        if (existingBook == null)
        {
            throw new KeyNotFoundException("Book not found.");
        }

        var bookReadDto = new BookReadDto
        {
            Id = existingBook.Id,
            Title = bookUpdateDto.Title,
            Isbn = bookUpdateDto.Isbn,
            PublishedAt = bookUpdateDto.PublishedAt
        };

        var isValid = await ValidateBookAsync(authorIds, categoryIds, bookReadDto);
        if (!isValid)
        {
            throw new InvalidOperationException("Book validation failed.");
        }

        existingBook.Title = bookUpdateDto.Title ?? existingBook.Title;
        existingBook.Isbn = bookUpdateDto.Isbn ?? existingBook.Isbn;
        existingBook.PublishedAt = bookUpdateDto.PublishedAt != DateTimeOffset.UtcNow ? bookUpdateDto.PublishedAt : existingBook.PublishedAt;
        existingBook.BookAuthors = authorIds.Select(id => new BookAuthor { AuthorId = id, BookId = existingBook.Id }).ToList();
        existingBook.BookCategories = categoryIds.Select(id => new BookCategory { CategoryId = id, BookId = existingBook.Id }).ToList();

        _context.Books.Update(existingBook);
        await _context.SaveChangesAsync();

        return true;
    }

    public async Task<bool> DeleteBookAsync(int bookId)
    {
        if (!await BookIdExistsAsync(bookId))
        {
            throw new KeyNotFoundException("Book not found.");
        }

        var book = await _context.Books.FindAsync(bookId);

        if (book == null)
        {
            throw new KeyNotFoundException("Book not found.");
        }

        _context.Books.Remove(book);
        await _context.SaveChangesAsync();

        return true;
    }

    public async Task<bool> BookIdExistsAsync(int bookId)
    {
        return await _context.Books.AnyAsync(b => b.Id == bookId);
    }

    public async Task<bool> BookIsbnExistsAsync(string bookIsbn)
    {
        return await _context.Books.AnyAsync(b => b.Isbn == bookIsbn);
    }

    public async Task<bool> IsDuplicateBookAsync(int bookId, string bookIsbn)
    {
        return await _context.Books
            .AnyAsync(b => b.Isbn.Trim().ToUpper() == bookIsbn.Trim().ToUpper() && b.Id != bookId);
    }

    public async Task<bool> ValidateBookAsync(List<int> authorIds, List<int> categoryIds, BookReadDto bookReadDto)
    {
        var modelState = new ModelStateDictionary();

        if (bookReadDto == null)
        {
            modelState.AddModelError("", "Missing book details");
            return false;
        }

        if (authorIds == null || authorIds.Count == 0)
        {
            modelState.AddModelError("", "Missing author IDs");
            return false;
        }

        if (categoryIds == null || categoryIds.Count == 0)
        {
            modelState.AddModelError("", "Missing category IDs");
            return false;
        }

        if (await _context.Books.AnyAsync(b => b.Id != bookReadDto.Id && b.Isbn == bookReadDto.Isbn))
        {
            modelState.AddModelError("", "Duplicate ISBN");
            return false;
        }

        var existingAuthors = await _context.Authors.Where(a => authorIds.Contains(a.Id)).Select(a => a.Id).ToListAsync();
        if (existingAuthors.Count != authorIds.Count)
        {
            modelState.AddModelError("", "One or more authors not found");
            return false;
        }

        var existingCategories = await _context.Categories.Where(c => categoryIds.Contains(c.Id)).Select(c => c.Id).ToListAsync();
        if (existingCategories.Count != categoryIds.Count)
        {
            modelState.AddModelError("", "One or more categories not found");
            return false;
        }

        return modelState.IsValid;
    }

    private async Task<Book?> GetBookIncludingDetailsAsync(Expression<Func<Book, bool>> predicate)
    {
        return await _context.Books.AsNoTracking()
            .Include(b => b.BookAuthors).ThenInclude(ba => ba.Author)
            .Include(b => b.BookCategories).ThenInclude(bc => bc.Category)
            .FirstOrDefaultAsync(predicate);
    }

    private BookReadDto MapToBookReadDto(Book book)
    {
        return new BookReadDto
        {
            Id = book.Id,
            Title = book.Title,
            Isbn = book.Isbn,
            PublishedAt = book.PublishedAt
        };
    }
}
