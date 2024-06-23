namespace Server.DTOs;

public class BookDto
{
    public int Id { get; set; }
    public string Isbn { get; set; } = string.Empty;
    public string Title { get; set; } = string.Empty;
    public DateTimeOffset PublishedAt { get; set; }
    public ICollection<int>? AuthorIds { get; set; }
    public ICollection<string>? AuthorNames { get; set; }
    public ICollection<int>? GenreIds { get; set; }
    public ICollection<string>? GenreNames { get; set; }
}
