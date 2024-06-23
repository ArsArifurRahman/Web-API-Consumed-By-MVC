namespace Server.DTOs.Book;

public class BookReadDto
{
    public string Isbn { get; set; } = string.Empty;
    public string Title { get; set; } = string.Empty;
    public DateTimeOffset PublishedAt { get; set; }
}
