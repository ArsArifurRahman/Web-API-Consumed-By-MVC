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
        try
        {
            // Retrieve all countries from the database
            var countries = await _context.Countries.ToListAsync();

            // Check if the countries list is null
            if (countries == null)
            {
                throw new InvalidOperationException("Failed to retrieve countries.");
            }

            // Convert the countries to CountryListDto objects
            var countryListDto = countries.Select(country => new CountryListDto
            {
                Name = country.Name ?? string.Empty
            });

            return countryListDto;
        }
        catch (Exception ex)
        {
            throw new Exception("Failed to retrieve countries.", ex);
        }
    }

    public async Task<CountryReadDto> GetCountryAsync(int id)
    {
        try
        {
            // Retrieve the country from the database
            var country = await _context.Countries.FirstOrDefaultAsync(x => x.Id == id);

            // Check if the country exists
            if (country == null)
            {
                throw new KeyNotFoundException("The country with the given id was not found.");
            }

            // Convert the country to a CountryReadDto object
            var countryReadDto = new CountryReadDto
            {
                Id = country.Id,
                Name = country.Name ?? string.Empty
            };

            return countryReadDto;
        }
        catch (Exception ex)
        {
            throw new Exception("Failed to retrieve country.", ex);
        }
    }

    public async Task<CountryCreateDto> AddCountryAsync(CountryCreateDto countryCreateDto)
    {
        try
        {
            // Check if the countryCreateDto is null
            ArgumentNullException.ThrowIfNull(countryCreateDto);

            // Create a new country
            var country = new Country { Name = countryCreateDto.Name };

            // Add the country to the database
            await _context.Countries.AddAsync(country);

            // Save the changes to the database
            await _context.SaveChangesAsync();

            // Convert the country to a CountryCreateDto object
            return new CountryCreateDto { Name = country.Name };
        }
        catch (Exception ex)
        {
            throw new Exception("Failed to add country.", ex);
        }
    }

    public async Task EditCountryAsync(int id, CountryUpdateDto countryUpdateDto)
    {
        try
        {
            // Check if the countryUpdateDto is null
            ArgumentNullException.ThrowIfNull(countryUpdateDto);

            // Retrieve the country from the database
            var existingCountry = await GetCountryAsync(id);

            // Update the country's name
            existingCountry.Name = countryUpdateDto.Name ?? existingCountry.Name;

            // Save the changes to the database
            await _context.SaveChangesAsync();
        }
        catch (Exception ex)
        {
            throw new Exception("Failed to edit country.", ex);
        }
    }

    public async Task DeleteCountryAsync(int id)
    {
        try
        {
            // Retrieve the country from the database
            var country = await _context.Countries.FirstOrDefaultAsync(x => x.Id == id);

            // Check if the country exists
            if (country == null)
            {
                throw new KeyNotFoundException("The country with the given id was not found.");
            }

            // Remove the country from the database
            _context.Countries.Remove(country);

            // Save the changes to the database
            await _context.SaveChangesAsync();
        }
        catch (Exception ex)
        {
            throw new Exception("Failed to delete country.", ex);
        }
    }
}