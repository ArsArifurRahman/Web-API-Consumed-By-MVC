namespace Server.DTOs.Relations;

public class ReviewerOfAReview
{
    public int Id { get; set; }
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public int ReviewId { get; set; }
}
