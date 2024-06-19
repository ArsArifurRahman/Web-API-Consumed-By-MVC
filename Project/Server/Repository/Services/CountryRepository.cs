using Microsoft.EntityFrameworkCore;
using Server.Data;
using Server.DTOs.Author;
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
            throw new ArgumentNullException(nameof(countryCreateDto), "CountryCreateDto cannot be null.");
        }

        if (await IsDuplicateCountryAsync(0, countryCreateDto.Name))
        {
            throw new InvalidOperationException("Country name already exists.");
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
            throw new ArgumentNullException(nameof(countryUpdateDto), "CountryUpdateDto cannot be null.");
        }

        if (!await CountryExistsAsync(id))
        {
            throw new KeyNotFoundException("Country not found.");
        }

        if (await IsDuplicateCountryAsync(id, countryUpdateDto.Name))
        {
            throw new InvalidOperationException("Country name already exists.");
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
        if (!await CountryExistsAsync(id))
        {
            throw new KeyNotFoundException("Country not found.");
        }

        var country = await _context.Countries.FindAsync(id);

        if (country == null)
        {
            throw new KeyNotFoundException("Country not found.");
        }

        _context.Countries.Remove(country);
        await _context.SaveChangesAsync();

        return true;
    }

    public async Task<CountryReadDto> GetCountryByAuthorAsync(int authorId)
    {
        var country = await _context.Authors
            .Where(a => a.Id == authorId)
            .Select(a => new CountryReadDto
            {
                Id = a.Country != null ? a.Country.Id : 0,
                Name = a.Country != null ? a.Country.Name : string.Empty
            })
            .FirstOrDefaultAsync();

        if (country == null || country.Id == 0)
        {
            throw new KeyNotFoundException($"Country not found for author with ID {authorId}.");
        }

        return country;
    }

    public async Task<IEnumerable<AuthorReadDto>> GetAuthorsByCountryAsync(int countryId)
    {
        if (!await CountryExistsAsync(countryId))
        {
            throw new KeyNotFoundException("Country not found.");
        }

        var authors = await _context.Authors
            .Where(a => a.CountryId == countryId)
            .ToListAsync();

        if (authors == null || !authors.Any())
        {
            throw new KeyNotFoundException("No authors found for the specified country.");
        }

        return authors.Select(author => new AuthorReadDto
        {
            Id = author.Id,
            FirstName = author.FirstName,
            LastName = author.LastName,
            CountryId = author.CountryId
        });
    }

    public async Task<bool> CountryExistsAsync(int countryId)
    {
        return await _context.Countries.AnyAsync(c => c.Id == countryId);
    }

    public async Task<bool> IsDuplicateCountryAsync(int countryId, string countryName)
    {
        return await _context.Countries
            .AnyAsync(c => c.Name.Trim().ToUpper() == countryName.Trim().ToUpper() && c.Id != countryId);
    }
}
