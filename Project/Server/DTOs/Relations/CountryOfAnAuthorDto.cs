using Server.DTOs.Country;

namespace Server.DTOs.Relations;

public class CountryOfAnAuthorDto
{
    public int Id { get; set; }
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public CountryReadDto Country { get; set; } = new CountryReadDto();
}
