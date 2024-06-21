namespace Server.DTOs.Book;

public class BookUpdateDto
{
    public int Id { get; set; }
    public string Isbn { get; set; } = string.Empty;
    public string Title { get; set; } = string.Empty;
    public DateTimeOffset PublishedAt { get; set; } = DateTimeOffset.UtcNow;
}
