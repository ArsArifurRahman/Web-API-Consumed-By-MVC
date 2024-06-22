namespace Server.DTOs.Review;

public class ReviewCreateDto
{
    public string Headline { get; set; } = string.Empty;
    public string ReviewText { get; set; } = string.Empty;
    public int Rating { get; set; }
    public int BookId { get; set; }
    public int ReviewerId { get; set; }
}
