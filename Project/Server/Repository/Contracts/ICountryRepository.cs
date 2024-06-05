using Server.Models;

namespace Server.Repository.Contracts;

public interface ICountryRepository
{
    Task<IEnumerable<Country>> GetCountriesAsync();
    Task<Country> GetCountryAsync(int id);
    Task<Country> AddCountryAsync(Country country);
    Task EditCountryAsync(int id, Country country);
    Task DeleteCountryAsync(int id);
    Task<Country> GetCountryOfAnAuthorAsync(int authorId);
    Task<IEnumerable<Author>> GetAuthorsFromACountryAsync(int id);
    Task<bool> IsCountryDuplicate(int countryId, string countryName);
    Task<bool> IsCountryExists(int countryId);
}
