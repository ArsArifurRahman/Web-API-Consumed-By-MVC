namespace Server.DTOs.Relations;

public class ReviewsOfAReviewer
{
    public int Id { get; set; }
    public string Headline { get; set; } = string.Empty;
    public string ReviewText { get; set; } = string.Empty;
    public int Rating { get; set; }
    public int BookId { get; set; }
}
