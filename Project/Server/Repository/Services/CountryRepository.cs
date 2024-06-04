using Microsoft.EntityFrameworkCore;
using Server.Data;
using Server.Models;
using Server.Repository.Contracts;

namespace Server.Repository.Services;

public class CountryRepository : ICountryRepository
{
    private readonly DataContext _context;

    public CountryRepository(DataContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Country>> GetCountriesAsync()
    {
        return await _context.Countries.ToListAsync();
    }

    public async Task<Country> GetCountryAsync(int id)
    {
        var country = await _context.Countries.FirstOrDefaultAsync(x => x.Id == id);

        if (country == null)
        {
            throw new InvalidOperationException("Country not found!");
        }

        return country;
    }

    public async Task<Country> AddCountryAsync(Country country)
    {
        if (country == null)
        {
            throw new ArgumentNullException(nameof(country));
        }

        await _context.Countries.AddAsync(country);
        await _context.SaveChangesAsync();

        return country;
    }

    public async Task EditCountryAsync(int id, Country country)
    {
        if (country == null)
        {
            throw new ArgumentNullException(nameof(country));
        }

        var existingCountry = await GetCountryAsync(id);

        if (existingCountry == null)
        {
            throw new KeyNotFoundException("The existing country with the given id was not found.");
        }

        _context.Entry(existingCountry).CurrentValues.SetValues(country);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteCountryAsync(int id)
    {
        var country = await GetCountryAsync(id);
        _context.Countries.Remove(country);
        await _context.SaveChangesAsync();
    }

    public Task<IEnumerable<Author>> GetAuthorsFromACountryAsync(int id)
    {
        throw new NotImplementedException();
    }

    public Task<Country> GetCountryOfAnAuthorAsync(int authorId)
    {
        throw new NotImplementedException();
    }
}
