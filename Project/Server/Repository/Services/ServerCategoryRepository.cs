using Microsoft.EntityFrameworkCore;
using Server.Data;
using Server.DTOs.Book;
using Server.DTOs.Category;
using Server.DTOs.Relations;
using Server.Models;
using Server.Repository.Contracts;

namespace Server.Repository.Services;

public class ServerCategoryRepository : IServerCategoryRepository
{
    private readonly DataContext _context;

    public ServerCategoryRepository(DataContext context)
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));
    }

    public async Task<IEnumerable<CategoryListDto>> GetCategoriesAsync()
    {
        var categories = await _context.Categories.AsNoTracking().ToListAsync();

        if (categories == null || !categories.Any())
        {
            throw new InvalidOperationException("No categories found.");
        }

        return categories.Select(category => new CategoryListDto
        {
            Name = category.Name
        }).ToList();
    }

    public async Task<CategoryReadDto> GetCategoryAsync(int id)
    {
        var category = await _context.Categories.AsNoTracking().FirstOrDefaultAsync(x => x.Id == id);

        if (category == null)
        {
            throw new KeyNotFoundException("Category not found.");
        }

        return new CategoryReadDto
        {
            Id = category.Id,
            Name = category.Name
        };
    }

    public async Task<CategoryReadDto> AddCategoryAsync(CategoryCreateDto categoryCreateDto)
    {
        if (categoryCreateDto == null)
        {
            throw new ArgumentNullException(nameof(categoryCreateDto), "CategoryCreateDto cannot be null.");
        }

        if (await IsDuplicateCategoryAsync(0, categoryCreateDto.Name))
        {
            throw new InvalidOperationException("Category name already exists.");
        }

        var category = new Category
        {
            Name = categoryCreateDto.Name
        };

        await _context.Categories.AddAsync(category);
        await _context.SaveChangesAsync();

        return new CategoryReadDto
        {
            Id = category.Id,
            Name = category.Name
        };
    }

    public async Task<bool> EditCategoryAsync(int id, CategoryUpdateDto categoryUpdateDto)
    {
        if (categoryUpdateDto == null)
        {
            throw new ArgumentNullException(nameof(categoryUpdateDto), "CategoryUpdateDto cannot be null.");
        }

        if (!await CategoryExistsAsync(id))
        {
            throw new KeyNotFoundException("Category not found.");
        }

        if (await IsDuplicateCategoryAsync(id, categoryUpdateDto.Name))
        {
            throw new InvalidOperationException("Category name already exists.");
        }

        var category = await _context.Categories.FindAsync(id);

        if (category == null)
        {
            throw new KeyNotFoundException("Category not found.");
        }

        category.Name = categoryUpdateDto.Name ?? category.Name;

        _context.Categories.Update(category);
        await _context.SaveChangesAsync();

        return true;
    }

    public async Task<bool> DeleteCategoryAsync(int id)
    {
        if (!await CategoryExistsAsync(id))
        {
            throw new KeyNotFoundException("Category not found.");
        }

        var category = await _context.Categories.FindAsync(id);

        if (category == null)
        {
            throw new KeyNotFoundException("Category not found.");
        }

        _context.Categories.Remove(category);
        await _context.SaveChangesAsync();

        return true;
    }

    public async Task<bool> CategoryExistsAsync(int categoryId)
    {
        return await _context.Categories.AnyAsync(c => c.Id == categoryId);
    }

    public async Task<bool> IsDuplicateCategoryAsync(int categoryId, string categoryName)
    {
        return await _context.Categories
            .AnyAsync(c => c.Name.Trim().ToUpper() == categoryName.Trim().ToUpper() && c.Id != categoryId);
    }

    public async Task<IEnumerable<BooksFromACategoryDto>> GetBooksFromACategoryAsync(int categoryId)
    {
        var category = await _context.Categories
            .Include(c => c.BookCategories)
            .ThenInclude(bc => bc.Book)
            .FirstOrDefaultAsync(c => c.Id == categoryId);

        if (category == null)
        {
            throw new KeyNotFoundException($"Category with Id {categoryId} not found.");
        }

        var booksFromACategoryDto = new BooksFromACategoryDto
        {
            Id = category.Id,
            Name = category.Name,
            Books = category.BookCategories
                .Where(bc => bc.Book != null)
                .Select(bc => new BookListDto
                {
                    Isbn = bc.Book!.Isbn,
                    Title = bc.Book!.Title,
                    PublishedAt = bc.Book!.PublishedAt
                })
                .ToList()
        };

        return new List<BooksFromACategoryDto> { booksFromACategoryDto };
    }

    public async Task<IEnumerable<CategoriesOfABookDto>> GetCategoriesOfABookAsync(int bookId)
    {
        var book = await _context.Books
            .Include(b => b.BookCategories)
            .ThenInclude(bc => bc.Category)
            .FirstOrDefaultAsync(b => b.Id == bookId);

        if (book == null)
        {
            throw new KeyNotFoundException($"Book with Id {bookId} not found.");
        }

        var categoriesOfABookDto = new CategoriesOfABookDto
        {
            Id = book.Id,
            Isbn = book.Isbn,
            Title = book.Title,
            PublishedAt = book.PublishedAt,
            Categories = book.BookCategories
                .Where(bc => bc.Category != null)
                .Select(bc => new CategoryListDto
                {
                    Name = bc.Category!.Name
                })
                .ToList()
        };

        return new List<CategoriesOfABookDto> { categoriesOfABookDto };
    }
}
