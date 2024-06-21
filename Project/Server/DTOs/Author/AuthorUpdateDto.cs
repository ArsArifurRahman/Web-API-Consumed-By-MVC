namespace Server.DTOs.Author;

public class AuthorUpdateDto
{
    public int Id { get; set; }
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public int CountryId { get; set; }
}
