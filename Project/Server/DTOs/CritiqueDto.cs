namespace Server.DTOs;

public class CritiqueDto
{
    public int Id { get; set; }
    public string Headline { get; set; } = string.Empty;
    public string ReviewText { get; set; } = string.Empty;
    public int Rating { get; set; }
    public int BookId { get; set; }
    public string BookTitle { get; set; } = string.Empty;
    public int ReviewerId { get; set; }
    public string ReviewerFullName { get; set; } = string.Empty;
}
