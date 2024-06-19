using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Server.Models;

public class Book
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }

    [Required(ErrorMessage = "ISBN is required.")]
    [StringLength(10, MinimumLength = 5, ErrorMessage = "ISBN should be between 5 and 10 characters.")]
    public string Isbn { get; set; } = string.Empty;

    [Required(ErrorMessage = "Title is required.")]
    [StringLength(32, ErrorMessage = "Title cannot be longer than 32 characters.")]
    public string Title { get; set; } = string.Empty;

    public DateTimeOffset PublishedAt { get; set; } = DateTimeOffset.UtcNow;

    public virtual ICollection<Review> Reviews { get; set; }
    public virtual ICollection<BookAuthor> BookAuthors { get; set; }
    public virtual ICollection<BookCategory> BookCategories { get; set; }

    public Book()
    {
        Reviews = new List<Review>();
        BookAuthors = new List<BookAuthor>();
        BookCategories = new List<BookCategory>();
    }
}
