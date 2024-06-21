using Server.DTOs.Book;

namespace Server.Repository.Contracts;

public interface IServerBookRepository
{
    Task<IEnumerable<BookListDto>> GetBooksAsync();
    Task<BookReadDto> GetBookByIdAsync(int bookId);
    Task<BookReadDto> GetBookByIsbnAsync(string bookIsbn);
    Task<decimal> GetBookByRating(int bookId);
    Task<BookReadDto> AddBookAsync(List<int> authorIds, List<int> categoryIds, BookCreateDto bookCreateDto);
    Task<bool> EditBookAsync(List<int> authorIds, List<int> categoryIds, int bookId, BookUpdateDto bookUpdateDto);
    Task<bool> DeleteBookAsync(int bookId);
    Task<bool> BookIdExistsAsync(int bookId);
    Task<bool> BookIsbnExistsAsync(string bookIsbn);
    Task<bool> IsDuplicateBookAsync(int bookId, string bookIsbn);
    Task<bool> ValidateBookAsync(List<int> authorIds, List<int> categoryIds, BookReadDto bookReadDto);
}
