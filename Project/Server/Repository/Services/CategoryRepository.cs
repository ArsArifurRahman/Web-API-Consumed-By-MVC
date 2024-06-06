using Microsoft.EntityFrameworkCore;
using Server.Data;
using Server.Models;
using Server.Repository.Contracts;

namespace Server.Repository.Services;

public class CategoryRepository : ICategoryRepository
{
    private readonly DataContext _context;

    public CategoryRepository(DataContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Category>> GetCategoriesAsync()
    {
        return await _context.Categories.ToListAsync();
    }

    public async Task<Category> GetCategoryAsync(int id)
    {
        return await _context.Categories.FirstOrDefaultAsync(x => x.Id == id) ?? throw new InvalidOperationException("Category not found!");
    }

    public async Task<Category> AddCategoryAsync(Category category)
    {
        ArgumentNullException.ThrowIfNull(category);
        await _context.Categories.AddAsync(category);
        await _context.SaveChangesAsync();
        return category;
    }

    public async Task EditCategoryAsync(int id, Category category)
    {
        ArgumentNullException.ThrowIfNull(category);
        var existingCategory = await GetCategoryAsync(id) ?? throw new KeyNotFoundException("The existing category with the given id was not found.");
        _context.Entry(existingCategory).CurrentValues.SetValues(category);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteCategoryAsync(int id)
    {
        var category = await GetCategoryAsync(id);
        _context.Categories.Remove(category);
        await _context.SaveChangesAsync();
    }
}
