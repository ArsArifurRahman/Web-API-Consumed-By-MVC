using Microsoft.EntityFrameworkCore;
using Server.Data;
using Server.Models;
using Server.Repository.Contracts;

namespace Server.Repository.Services;

public class AuthorRepository : IAuthorRepository
{
    private readonly DataContext _context;

    public AuthorRepository(DataContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Author>> GetAuthorsAsync()
    {
        return await _context.Authors.ToListAsync();
    }

    public async Task<Author> GetAuthorAsync(int id)
    {
        return await _context.Authors.FirstOrDefaultAsync(x => x.Id == id) ?? throw new InvalidOperationException("Author not found.");
    }

    public async Task<Author> AddAuthorAsync(Author author)
    {
        ArgumentNullException.ThrowIfNull(author);
        await _context.Authors.AddAsync(author);
        await _context.SaveChangesAsync();
        return author;
    }

    public async Task EditAuthorAsync(int id, Author author)
    {
        ArgumentNullException.ThrowIfNull(author);
        var existingAuthor = await GetAuthorAsync(id) ?? throw new KeyNotFoundException("The existing author with the given id was not found.");
        _context.Entry(existingAuthor).CurrentValues.SetValues(author);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAuthorAsync(int id)
    {
        var author = await GetAuthorAsync(id);
        _context.Authors.Remove(author);
        await _context.SaveChangesAsync();
    }
}
