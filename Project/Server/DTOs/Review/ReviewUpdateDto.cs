namespace Server.DTOs.Review;

public class ReviewUpdateDto
{
    public int Id { get; set; }
    public string Headline { get; set; } = string.Empty;
    public string ReviewText { get; set; } = string.Empty;
    public int Rating { get; set; }
}
