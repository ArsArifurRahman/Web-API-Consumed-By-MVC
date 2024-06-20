using Server.DTOs.Category;

namespace Server.DTOs.Relations;

public class CategoriesOfABookDto
{
    public int Id { get; set; }
    public string Isbn { get; set; } = string.Empty;
    public string Title { get; set; } = string.Empty;
    public DateTimeOffset PublishedAt { get; set; } = DateTimeOffset.UtcNow;
    public ICollection<CategoryListDto> Categories { get; set; } = new List<CategoryListDto>();
}
