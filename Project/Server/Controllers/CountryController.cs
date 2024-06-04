﻿using Microsoft.AspNetCore.Mvc;
using Server.DTOs.Country;
using Server.Models;
using Server.Repository.Contracts;

namespace Server.Controllers;

[Route("api/[controller]")]
[ApiController]
public class CountryController : ControllerBase
{
    private readonly ICountryRepository _countryRepository;

    public CountryController(ICountryRepository countryRepository)
    {
        _countryRepository = countryRepository;
    }

    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult<IEnumerable<CountryListDto>>> GetCountries()
    {
        var countries = await _countryRepository.GetCountriesAsync();

        var countryListDto = countries.Select(country => new CountryListDto
        {
            Name = country.Name ?? string.Empty
        });

        return Ok(countryListDto);
    }


    [HttpGet("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<CountryReadDto>> GetCountry(int id)
    {
        var country = await _countryRepository.GetCountryAsync(id);

        if (country == null)
        {
            return NotFound("Country not found.");
        }

        var countryReadDto = new CountryReadDto
        {
            Id = country.Id,
            Name = country.Name ?? string.Empty
        };

        return Ok(countryReadDto);
    }


    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    public async Task<ActionResult<Country>> PostCountry(CountryCreateDto countryCreateDto)
    {
        var country = new Country
        {
            Name = countryCreateDto.Name
        };

        var createdCountry = await _countryRepository.AddCountryAsync(country);
        return CreatedAtAction(nameof(GetCountry), new { id = createdCountry.Id }, createdCountry);
    }

    [HttpPut("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> PutCountry(int id, CountryUpdateDto countryUpdateDto)
    {
        if (id != countryUpdateDto.Id)
        {
            return BadRequest("Country ID mismatch.");
        }

        var country = await _countryRepository.GetCountryAsync(id);

        if (country == null)
        {
            return NotFound("Country not found.");
        }

        country.Name = countryUpdateDto.Name ?? country.Name;
        await _countryRepository.EditCountryAsync(id, country);

        return NoContent();
    }

    [HttpDelete("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> DeleteCountry(int id)
    {
        var country = await _countryRepository.GetCountryAsync(id);
        if (country == null)
        {
            return NotFound("Country not found.");
        }

        await _countryRepository.GetCountryAsync(id);

        return NoContent();
    }
}
