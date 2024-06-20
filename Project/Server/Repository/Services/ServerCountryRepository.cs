using Microsoft.EntityFrameworkCore;
using Server.Data;
using Server.DTOs.Author;
using Server.DTOs.Country;
using Server.DTOs.Relations;
using Server.Models;
using Server.Repository.Contracts;

namespace Server.Repository.Services;

public class ServerCountryRepository : IServerCountryRepository
{
    private readonly DataContext _context;

    public ServerCountryRepository(DataContext context)
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

    public async Task<CountryOfAnAuthorDto> GetCountryOfAnAuthorAsync(int authorId)
    {
        var author = await _context.Authors
            .Include(a => a.Country)
            .FirstOrDefaultAsync(a => a.Id == authorId);

        if (author == null)
        {
            throw new KeyNotFoundException($"Author with Id {authorId} not found.");
        }

        if (author.Country == null)
        {
            throw new InvalidOperationException($"Author with Id {authorId} does not have an associated country.");
        }

        var countryOfAnAuthorDto = new CountryOfAnAuthorDto
        {
            Id = author.Id,
            FirstName = author.FirstName,
            LastName = author.LastName,
            Country = new CountryReadDto
            {
                Id = author.Country.Id,
                Name = author.Country.Name
            }
        };

        return countryOfAnAuthorDto;
    }


    public async Task<IEnumerable<AuthorsfromACountryDto>> GetAuthorsFromACountryAsync(int countryId)
    {
        var country = await _context.Countries
            .Include(c => c.Authors)
            .FirstOrDefaultAsync(c => c.Id == countryId);

        if (country == null)
        {
            throw new KeyNotFoundException($"Country with Id {countryId} not found.");
        }

        var authorsFromACountryDto = new AuthorsfromACountryDto
        {
            Id = country.Id,
            Name = country.Name,
            Authors = country.Authors
                .Select(a => new AuthorListDto
                {
                    FirstName = a.FirstName,
                    LastName = a.LastName
                })
                .ToList()
        };

        return new List<AuthorsfromACountryDto> { authorsFromACountryDto };
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
