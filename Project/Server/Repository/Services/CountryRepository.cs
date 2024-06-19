using Microsoft.EntityFrameworkCore;
using Server.Data;
using Server.DTOs.Country;
using Server.Models;
using Server.Repository.Contracts;

namespace Server.Repository.Services;

public class CountryRepository : ICountryRepository
{
    private readonly DataContext _context;

    public CountryRepository(DataContext context)
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));
    }

    public async Task<IEnumerable<CountryListDto>> GetCountriesAsync()
    {
        var countries = await _context.Countries.AsNoTracking().ToListAsync();

        if (countries == null || !countries.Any())
        {
            throw new InvalidOperationException("No countries found.");
        }

        return countries.Select(country => new CountryListDto
        {
            Name = country.Name
        }).ToList();
    }

    public async Task<CountryReadDto> GetCountryAsync(int id)
    {
        var country = await _context.Countries.AsNoTracking().FirstOrDefaultAsync(x => x.Id == id);

        if (country == null)
        {
            throw new KeyNotFoundException("Country not found.");
        }

        return new CountryReadDto
        {
            Id = country.Id,
            Name = country.Name
        };
    }

    public async Task<CountryReadDto> AddCountryAsync(CountryCreateDto countryCreateDto)
    {
        if (countryCreateDto == null)
        {
            throw new ArgumentNullException(nameof(countryCreateDto));
        }

        var country = new Country
        {
            Name = countryCreateDto.Name
        };

        await _context.Countries.AddAsync(country);
        await _context.SaveChangesAsync();

        return new CountryReadDto
        {
            Id = country.Id,
            Name = country.Name
        };
    }

    public async Task<bool> EditCountryAsync(int id, CountryUpdateDto countryUpdateDto)
    {
        if (countryUpdateDto == null)
        {
            throw new ArgumentNullException(nameof(countryUpdateDto));
        }

        var country = await _context.Countries.FindAsync(id);

        if (country == null)
        {
            throw new KeyNotFoundException("Country not found.");
        }

        country.Name = countryUpdateDto.Name ?? country.Name;

        _context.Countries.Update(country);
        await _context.SaveChangesAsync();

        return true;
    }

    public async Task<bool> DeleteCountryAsync(int id)
    {
        var country = await _context.Countries.FindAsync(id);

        if (country == null)
        {
            throw new KeyNotFoundException("Country not found.");
        }

        _context.Countries.Remove(country);
        await _context.SaveChangesAsync();

        return true;
    }
}
