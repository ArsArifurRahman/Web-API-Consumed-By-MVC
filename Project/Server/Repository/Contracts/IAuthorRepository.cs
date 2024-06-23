using Server.DTOs;
using Server.DTOs.Author;
using Server.DTOs.Book;

namespace Server.Repository.Contracts;

public interface IAuthorRepository
{
    Task<IEnumerable<AuthorReadDto>> GetAuthorsAsync();
    Task<AuthorDto> GetAuthorAsync(int authorId);
    Task<IEnumerable<AuthorReadDto>> GetAuthorsOfABookAsync(int bookId);
    Task<IEnumerable<BookReadDto>> GetBooksOfAnAuthorAsync(int authorId);
    Task<AuthorDto> CreateAuthorAsync(AuthorAddOrEditDto authorAddOrEditDto);
    Task<bool> UpdateAuthorAsync(int authorId, AuthorAddOrEditDto authorAddOrEditDto);
    Task<bool> DeleteAuthorAsync(int authorId);
}
