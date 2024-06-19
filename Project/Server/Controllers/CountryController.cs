﻿using Microsoft.AspNetCore.Mvc;
using Server.DTOs.Country;
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
        return Ok(countries);
    }

    [HttpGet("{countryId}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<CountryReadDto>> GetCountry(int countryId)
    {
        var country = await _countryRepository.GetCountryAsync(countryId);
        if (country == null)
        {
            return NotFound();
        }
        return Ok(country);
    }

    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<CountryCreateDto>> PostCountry(CountryCreateDto countryCreateDto)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var createdCountry = await _countryRepository.AddCountryAsync(countryCreateDto);
        return CreatedAtAction(nameof(GetCountry), new { countryId = createdCountry.Id }, createdCountry);
    }

    [HttpPut("{countryId}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> PutCountry(int countryId, CountryUpdateDto countryUpdateDto)
    {
        if (countryId != countryUpdateDto.Id)
        {
            return BadRequest("Country ID mismatch.");
        }

        await _countryRepository.EditCountryAsync(countryId, countryUpdateDto);
        return NoContent();
    }

    [HttpDelete("{countryId}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> DeleteCountry(int countryId)
    {
        try
        {
            await _countryRepository.DeleteCountryAsync(countryId);
            return NoContent();
        }
        catch (KeyNotFoundException)
        {
            return NotFound();
        }
    }
}
