using Server.DTOs.Country;
using Server.DTOs.Relations;

namespace Server.Repository.Contracts;

public interface ICountryRepository
{
    Task<IEnumerable<CountryListDto>> GetCountriesAsync();
    Task<CountryReadDto> GetCountryAsync(int id);
    Task<CountryReadDto> AddCountryAsync(CountryCreateDto countryCreateDto);
    Task<bool> EditCountryAsync(int id, CountryUpdateDto countryUpdateDto);
    Task<bool> DeleteCountryAsync(int id);
    Task<bool> CountryExistsAsync(int countryId);
    Task<bool> IsDuplicateCountryAsync(int countryId, string countryName);
    Task<CountryOfAnAuthorDto> GetCountryOfAnAuthorAsync(int authorId);
    Task<IEnumerable<AuthorsfromACountryDto>> GetAuthorsFromACountryAsync(int countryId);
}