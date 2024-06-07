using Server.DTOs.Country;

namespace Server.Repository.Contracts;

public interface ICountryRepository
{
    Task<IEnumerable<CountryListDto>> GetCountriesAsync();
    Task<CountryReadDto> GetCountryAsync(int id);
    Task<CountryCreateDto> AddCountryAsync(CountryCreateDto countryCreateDto);
    Task EditCountryAsync(int id, CountryUpdateDto countryUpdateDto);
    Task DeleteCountryAsync(int id);
}
