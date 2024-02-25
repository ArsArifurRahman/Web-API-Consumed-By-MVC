using Backend.Entities.ViewModels;
using Backend.Repository.Contracts;
using Microsoft.AspNetCore.Mvc;

namespace Backend.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CountryController : ControllerBase
{
    private readonly ICountryRepository _countryRepository;

    public CountryController(ICountryRepository countryRepository)
    {
        _countryRepository = countryRepository;
    }

    [HttpGet]
    [ProducesResponseType(400)]
    [ProducesResponseType(200, Type = typeof(IEnumerable<CountryViewModel>))]
    public IActionResult GetCountries()
    {
        var countriesModel = _countryRepository.GetCountries().ToList();

        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var countriesViewModel = new List<CountryViewModel>();

        foreach (var item in countriesModel)
        {
            countriesViewModel.Add(new CountryViewModel { Id = item.Id, Name = item.Name, });
        }

        return Ok(countriesViewModel);
    }

    [HttpGet("{countryId}")]
    [ProducesResponseType(400)]
    [ProducesResponseType(404)]
    [ProducesResponseType(200, Type = typeof(CountryViewModel))]
    public IActionResult GetCountry(int? countryId)
    {
        if (!_countryRepository.CountryExists(countryId))
        {
            return NotFound();
        }

        var countryModel = _countryRepository.GetCountry(countryId);

        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var countryViewModel = new CountryViewModel()
        {
            Id = countryModel.Id,
            Name = countryModel.Name,
        };

        return Ok(countryViewModel);
    }

    [HttpGet("author/{authorId}")]
    [ProducesResponseType(400)]
    [ProducesResponseType(404)]
    [ProducesResponseType(200, Type = typeof(CountryViewModel))]
    public IActionResult GetCountryOfAnAuthor(int? authorId)
    {
        var countryModel = _countryRepository.GetCountryOfAnAuthor(authorId);

        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var countryViewModel = new CountryViewModel()
        {
            Id = countryModel.Id,
            Name = countryModel.Name,
        };

        return Ok(countryViewModel);
    }
}
