using Server.Models;

namespace Server.Repository.Contracts;

public interface ICategoryRepository
{
    Task<IEnumerable<Category>> GetCategoriesAsync();
    Task<Category> GetCategoryAsync(int id);
    Task<Category> AddCategoryAsync(Category category);
    Task EditCategoryAsync(int id, Category category);
    Task DeleteCategoryAsync(int id);
}
