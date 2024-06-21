using Microsoft.EntityFrameworkCore;
using Server.Data;
using Server.DTOs.Author;
using Server.DTOs.Book;
using Server.Models;
using Server.Repository.Contracts;

namespace Server.Repository.Services
{
    public class ServerAuthorRepository : IServerAuthorRepository
    {
        private readonly DataContext _context;

        public ServerAuthorRepository(DataContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public async Task<IEnumerable<AuthorListDto>> GetAuthorsAsync()
        {
            try
            {
                var authors = await _context.Authors
                    .AsNoTracking()
                    .Select(a => new AuthorListDto
                    {
                        FirstName = a.FirstName,
                        LastName = a.LastName,
                        CountryName = a.Country != null ? a.Country.Name : string.Empty
                    })
                    .ToListAsync();

                return authors;
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while retrieving authors.", ex);
            }
        }

        public async Task<AuthorReadDto> GetAuthorAsync(int authorId)
        {
            try
            {
                var author = await _context.Authors
                    .AsNoTracking()
                    .Include(a => a.Country)
                    .SingleOrDefaultAsync(a => a.Id == authorId);

                if (author == null)
                    throw new KeyNotFoundException($"Author with ID {authorId} not found.");

                return new AuthorReadDto
                {
                    Id = author.Id,
                    FirstName = author.FirstName,
                    LastName = author.LastName,
                    CountryName = author.Country != null ? author.Country.Name : string.Empty
                };
            }
            catch (KeyNotFoundException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new Exception($"An error occurred while retrieving author with ID {authorId}.", ex);
            }
        }

        public async Task<IEnumerable<AuthorListDto>> GetAuthorsOfABookAsync(int bookId)
        {
            try
            {
                var authors = await _context.BookAuthors
                    .AsNoTracking()
                    .Where(ba => ba.BookId == bookId && ba.Author != null)
                    .Select(ba => new AuthorListDto
                    {
                        FirstName = ba.Author!.FirstName ?? string.Empty,
                        LastName = ba.Author.LastName ?? string.Empty,
                        CountryName = ba.Author.Country != null ? ba.Author.Country.Name : string.Empty
                    })
                    .ToListAsync();

                return authors;
            }
            catch (Exception ex)
            {
                throw new Exception($"An error occurred while retrieving authors for book with ID {bookId}.", ex);
            }
        }

        public async Task<IEnumerable<BookListDto>> GetBooksOfAnAuthorAsync(int authorId)
        {
            try
            {
                var books = await _context.BookAuthors
                    .AsNoTracking()
                    .Where(ba => ba.AuthorId == authorId)
                    .Select(ba => new BookListDto
                    {
                        Title = ba.Book!.Title,
                        Isbn = ba.Book!.Isbn,
                        PublishedAt = ba.Book!.PublishedAt
                    })
                    .ToListAsync();

                return books;
            }
            catch (Exception ex)
            {
                throw new Exception($"An error occurred while retrieving books for author with ID {authorId}.", ex);
            }
        }

        public async Task<bool> AuthorExistsAsync(int authorId)
        {
            try
            {
                return await _context.Authors
                    .AnyAsync(a => a.Id == authorId);
            }
            catch (Exception ex)
            {
                throw new Exception($"An error occurred while checking if author with ID {authorId} exists.", ex);
            }
        }

        public async Task<AuthorReadDto> CreateAuthorAsync(AuthorCreateDto authorCreateDto)
        {
            try
            {
                var country = await _context.Countries.FindAsync(authorCreateDto.CountryId);
                if (country == null)
                {
                    throw new ArgumentException($"Country with ID {authorCreateDto.CountryId} not found.");
                }

                var author = new Author
                {
                    FirstName = authorCreateDto.FirstName,
                    LastName = authorCreateDto.LastName,
                    CountryId = authorCreateDto.CountryId
                };

                _context.Authors.Add(author);
                await _context.SaveChangesAsync();

                return new AuthorReadDto
                {
                    Id = author.Id,
                    FirstName = author.FirstName,
                    LastName = author.LastName,
                    CountryName = country.Name
                };
            }
            catch (ArgumentException ex)
            {
                throw new ArgumentException(ex.Message, ex);
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while creating the author.", ex);
            }
        }

        public async Task<bool> UpdateAuthorAsync(int authorId, AuthorUpdateDto authorUpdateDto)
        {
            try
            {
                var author = await _context.Authors.FindAsync(authorId);

                if (author == null)
                    throw new KeyNotFoundException($"Author with ID {authorId} not found.");

                var country = await _context.Countries.FindAsync(authorUpdateDto.CountryId);
                if (country == null)
                {
                    throw new ArgumentException($"Country with ID {authorUpdateDto.CountryId} not found.");
                }

                author.FirstName = authorUpdateDto.FirstName;
                author.LastName = authorUpdateDto.LastName;
                author.CountryId = authorUpdateDto.CountryId;

                _context.Authors.Update(author);
                await _context.SaveChangesAsync();

                return true;
            }
            catch (KeyNotFoundException)
            {
                throw;
            }
            catch (ArgumentException ex)
            {
                throw new ArgumentException(ex.Message, ex);
            }
            catch (Exception ex)
            {
                throw new Exception($"An error occurred while updating author with ID {authorId}.", ex);
            }
        }

        public async Task<bool> DeleteAuthorAsync(int authorId)
        {
            try
            {
                var author = await _context.Authors.FindAsync(authorId);

                if (author == null)
                    throw new KeyNotFoundException($"Author with ID {authorId} not found.");

                _context.Authors.Remove(author);
                await _context.SaveChangesAsync();

                return true;
            }
            catch (KeyNotFoundException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new Exception($"An error occurred while deleting author with ID {authorId}.", ex);
            }
        }
    }
}
