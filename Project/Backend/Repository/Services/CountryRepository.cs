using Backend.Data;
using Backend.Entities.Models;
using Backend.Repository.Contracts;

namespace Backend.Repository.Services;

public class CountryRepository : ICountryRepository
{
    private readonly DataContext _countryContext;

    public CountryRepository(DataContext countryContext)
    {
        _countryContext = countryContext;
    }

    public bool CountryExists(int? countryId)
    {
        return _countryContext.Countries.Any(x => x.Id == countryId);
    }

    public ICollection<Author> GetAuthorsFromACountry(int? countryId)
    {
        return _countryContext.Authors.Where(x => x.Country.Id == countryId).ToList();
    }

    public ICollection<Country> GetCountries()
    {
        return _countryContext.Countries.OrderBy(x => x.Name).ToList();
    }

    public Country GetCountry(int? countryId)
    {
        return _countryContext.Countries.Where(x => x.Id == countryId).FirstOrDefault();
    }

    public Country GetCountryOfAnAuthor(int? authorId)
    {
        return _countryContext
            .Authors.Where(x => x.Id == authorId)
            .Select(y => y.Country)
            .FirstOrDefault();
    }
}
