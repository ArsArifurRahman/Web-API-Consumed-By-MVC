using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Server.Models;

[Index(nameof(Isbn), IsUnique = true)]
[Index(nameof(Title), IsUnique = true)]
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

    [Required(ErrorMessage = "Published date is required.")]
    [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
    public DateTimeOffset PublishedAt { get; set; }

    public virtual ICollection<Critique> Reviews { get; set; }
    public virtual ICollection<BookAuthor> BookAuthors { get; set; }
    public virtual ICollection<BookGenre> BookGenres { get; set; }

    public Book()
    {
        Reviews = new List<Critique>();
        BookAuthors = new List<BookAuthor>();
        BookGenres = new List<BookGenre>();
    }
}
