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

    public Task<IEnumerable<Category>> GetCategoriesAsync()
    {
        throw new NotImplementedException();
    }

    public Task<Category> GetCategoryAsync(int id)
    {
        throw new NotImplementedException();
    }

    public Task<Category> AddCategoryAsync(Category category)
    {
        throw new NotImplementedException();
    }

    public Task EditCategoryAsync(int id, Category category)
    {
        throw new NotImplementedException();
    }

    public Task DeleteCategoryAsync(int id)
    {
        throw new NotImplementedException();
    }
}
