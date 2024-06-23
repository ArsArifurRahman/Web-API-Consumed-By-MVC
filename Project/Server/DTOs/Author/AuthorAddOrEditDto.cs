namespace Server.DTOs.Author;

public class AuthorAddOrEditDto
{
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public int CountryId { get; set; }
}
