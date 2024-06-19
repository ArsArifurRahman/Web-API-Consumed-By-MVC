using Server.DTOs.Country;

namespace Server.Repository.Contracts;

public interface ICountryRepository
{
    Task<IEnumerable<CountryListDto>> GetCountriesAsync();
    Task<CountryReadDto> GetCountryAsync(int id);
    Task<CountryReadDto> AddCountryAsync(CountryCreateDto countryCreateDto);
    Task<bool> EditCountryAsync(int id, CountryUpdateDto countryUpdateDto);
    Task<bool> DeleteCountryAsync(int id);
}