using Server.DTOs.Category;
using Server.DTOs.Relations;

namespace Server.Repository.Contracts;

public interface IServerCategoryRepository
{
    Task<IEnumerable<CategoryListDto>> GetCategoriesAsync();
    Task<CategoryReadDto> GetCategoryAsync(int id);
    Task<CategoryReadDto> AddCategoryAsync(CategoryCreateDto categoryCreateDto);
    Task<bool> EditCategoryAsync(int id, CategoryUpdateDto categoryUpdateDto);
    Task<bool> DeleteCategoryAsync(int id);
    Task<bool> CategoryExistsAsync(int categoryId);
    Task<bool> IsDuplicateCategoryAsync(int categoryId, string categoryName);
    Task<IEnumerable<BooksFromACategoryDto>> GetBooksFromACategoryAsync(int categoryId);
    Task<IEnumerable<CategoriesOfABookDto>> GetCategoriesOfABookAsync(int bookId);
}
