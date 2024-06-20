using Server.DTOs.Book;

namespace Server.DTOs.Relations;

public class BooksFromACategoryDto
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public ICollection<BookListDto> Books { get; set; } = new List<BookListDto>();
}
