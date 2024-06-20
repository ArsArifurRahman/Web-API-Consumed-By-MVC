using Server.DTOs.Author;

namespace Server.DTOs.Relations;

public class AuthorsfromACountryDto
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public ICollection<AuthorListDto> Authors { get; set; } = new List<AuthorListDto>();
}
