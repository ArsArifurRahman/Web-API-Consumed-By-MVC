using Microsoft.AspNetCore.Mvc;
using Server.DTOs.Category;
using Server.Models;
using Server.Repository.Contracts;

namespace Server.Controllers;

[Route("api/[controller]")]
[ApiController]
public class CategoryController : ControllerBase
{
    private readonly ICategoryRepository _categoryRepository;

    public CategoryController(ICategoryRepository categoryRepository)
    {
        _categoryRepository = categoryRepository;
    }

    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult<IEnumerable<CategoryListDto>>> GetCategories()
    {
        var categories = await _categoryRepository.GetCategoriesAsync();

        var categoryListDto = categories.Select(category => new CategoryListDto
        {
            Name = category.Name ?? string.Empty
        });

        return Ok(categoryListDto);
    }

    [HttpGet("{categoryId}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<CategoryReadDto>> GetCategory(int categoryId)
    {
        var category = await _categoryRepository.GetCategoryAsync(categoryId);

        if (category == null)
        {
            return NotFound("Category not found.");
        }

        var categoryReadDto = new CategoryReadDto
        {
            Id = category.Id,
            Name = category.Name ?? string.Empty
        };

        return Ok(categoryReadDto);
    }

    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    public async Task<ActionResult<Category>> PostCategory(CategoryCreateDto categoryCreateDto)
    {
        var category = new Category
        {
            Name = categoryCreateDto.Name
        };

        var createdCategory = await _categoryRepository.AddCategoryAsync(category);
        return CreatedAtAction(nameof(GetCategory), new { categoryId = createdCategory.Id }, createdCategory);
    }

    [HttpPut("{categoryId}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> PutCategory(int categoryId, CategoryUpdateDto categoryUpdateDto)
    {
        if (categoryId != categoryUpdateDto.Id)
        {
            return BadRequest("Category ID mismatch.");
        }

        var category = await _categoryRepository.GetCategoryAsync(categoryId);

        if (category == null)
        {
            return NotFound("Category not found.");
        }

        category.Name = categoryUpdateDto.Name ?? category.Name;
        await _categoryRepository.EditCategoryAsync(categoryId, category);

        return NoContent();
    }

    [HttpDelete("{categoryId}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> DeleteCategory(int categoryId)
    {
        try
        {
            await _categoryRepository.DeleteCategoryAsync(categoryId);
        }
        catch (KeyNotFoundException)
        {
            return NotFound();
        }

        return NoContent();
    }
}
