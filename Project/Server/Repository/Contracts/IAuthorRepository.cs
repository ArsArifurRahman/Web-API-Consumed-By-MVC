using Server.Models;

namespace Server.Repository.Contracts;

public interface IAuthorRepository
{
    Task<IEnumerable<Author>> GetAuthorsAsync();
    Task<Author> GetAuthorAsync(int id);
    Task<Author> AddAuthorAsync(Author author);
    Task EditAuthorAsync(int id, Author author);
    Task DeleteAuthorAsync(int id);
}
