namespace Server.DTOs.Relations;

public class ReviewsOfABook
{
    public int Id { get; set; }
    public string Headline { get; set; } = string.Empty;
    public string ReviewText { get; set; } = string.Empty;
    public int Rating { get; set; }
    public int ReviewerId { get; set; }
    public string ReviewerFullName { get; set; } = string.Empty;
}
