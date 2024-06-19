using Server.DTOs.Author;
using Server.DTOs.Country;

namespace Server.Repository.Contracts;

public interface ICountryRepository
{
    Task<IEnumerable<CountryListDto>> GetCountriesAsync();
    Task<CountryReadDto> GetCountryAsync(int id);
    Task<CountryReadDto> AddCountryAsync(CountryCreateDto countryCreateDto);
    Task<bool> EditCountryAsync(int id, CountryUpdateDto countryUpdateDto);
    Task<bool> DeleteCountryAsync(int id);
    Task<CountryReadDto> GetCountryByAuthorAsync(int authorId);
    Task<IEnumerable<AuthorReadDto>> GetAuthorsByCountryAsync(int countryId);
    Task<bool> CountryExistsAsync(int countryId);
    Task<bool> IsDuplicateCountryAsync(int countryId, string countryName);
}