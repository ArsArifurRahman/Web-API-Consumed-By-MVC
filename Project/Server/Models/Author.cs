using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Server.Models;

[Index(nameof(FirstName), nameof(LastName), IsUnique = true)]
public class Author
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }

    [Required(ErrorMessage = "First name is required.")]
    [StringLength(32, ErrorMessage = "First name cannot be longer than 32 characters.")]
    public string FirstName { get; set; } = string.Empty;

    [Required(ErrorMessage = "Last name is required.")]
    [StringLength(32, ErrorMessage = "Last name cannot be longer than 32 characters.")]
    public string LastName { get; set; } = string.Empty;

    [ForeignKey(nameof(Country))]
    public int CountryId { get; set; }
    public virtual Country? Country { get; set; }

    public virtual ICollection<BookAuthor> BookAuthors { get; set; }

    public Author()
    {
        BookAuthors = new List<BookAuthor>();
    }
}
