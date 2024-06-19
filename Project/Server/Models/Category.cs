using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Server.Models;

public class Category
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }

    [Required(ErrorMessage = "Name is required.")]
    [StringLength(32, ErrorMessage = "Name cannot be longer than 32 characters.")]
    public string Name { get; set; } = string.Empty;

    public virtual ICollection<BookCategory> BookCategories { get; set; }

    public Category()
    {
        BookCategories = new List<BookCategory>();
    }
}
