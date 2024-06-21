using Server.DTOs.Author;
using Server.DTOs.Book;

namespace Server.Repository.Contracts;

public interface IServerAuthorRepository
{
    Task<IEnumerable<AuthorListDto>> GetAuthorsAsync();
    Task<AuthorReadDto> GetAuthorAsync(int authorId);
    Task<IEnumerable<AuthorListDto>> GetAuthorsOfABookAsync(int bookId);
    Task<IEnumerable<BookListDto>> GetBooksOfAnAuthorAsync(int authorId);
    Task<bool> AuthorExistsAsync(int authorId);
    Task<AuthorReadDto> CreateAuthorAsync(AuthorCreateDto authorCreateDto);
    Task<bool> UpdateAuthorAsync(int authorId, AuthorUpdateDto authorUpdateDto);
    Task<bool> DeleteAuthorAsync(int authorId);
}
