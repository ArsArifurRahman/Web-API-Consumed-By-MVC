namespace Server.DTOs.Relations;

public class BookOfAReview
{
    public int Id { get; set; }
    public string Isbn { get; set; } = string.Empty;
    public string Title { get; set; } = string.Empty;
    public DateTimeOffset PublishedAt { get; set; }
    public int ReviewId { get; set; }
}
