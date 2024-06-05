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
        var country = await _context.Countries.FirstOrDefaultAsync(x => x.Id == id) ?? throw new InvalidOperationException("Country not found!");
        return country;
    }

    public async Task<Country> AddCountryAsync(Country country)
    {
        ArgumentNullException.ThrowIfNull(country);
        await _context.Countries.AddAsync(country);
        await _context.SaveChangesAsync();
        return country;
    }
    public async Task EditCountryAsync(int id, Country country)
    {
        ArgumentNullException.ThrowIfNull(country);
        var existingCountry = await GetCountryAsync(id) ?? throw new KeyNotFoundException("The existing country with the given id was not found.");
        _context.Entry(existingCountry).CurrentValues.SetValues(country);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteCountryAsync(int id)
    {
        var country = await GetCountryAsync(id);
        _context.Countries.Remove(country);
        await _context.SaveChangesAsync();
    }

    public async Task<IEnumerable<Author>> GetAuthorsFromACountryAsync(int id)
    {
        return await _context.Authors.Where(x => x.CountryId == id).ToListAsync();
    }

    public async Task<Country> GetCountryOfAnAuthorAsync(int authorId)
    {
        var country = await _context.Authors.Where(x => x.Id == authorId).Select(y => y.Country).FirstOrDefaultAsync() ?? throw new ArgumentException($"Country Author with ID {authorId} not found.");
        return country;
    }

    public async Task<bool> IsCountryDuplicate(int id, string countryName)
    {
        var country = await _context.Countries.Where(x => x.Name != null && x.Name.Trim().Equals(countryName.Trim(), StringComparison.CurrentCultureIgnoreCase) && x.Id != id).FirstOrDefaultAsync();
        return country != null;
    }

    public async Task<bool> IsCountryExists(int id)
    {
        var country = await _context.Countries.FirstOrDefaultAsync(x => x.Id == id);
        return country != null;
    }
}
