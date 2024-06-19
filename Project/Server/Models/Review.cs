using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Server.Models;

public class Review
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }

    [Required(ErrorMessage = "Headline is required.")]
    [StringLength(100, MinimumLength = 10, ErrorMessage = "Headline should be between 10 and 100 characters.")]
    public string Headline { get; set; } = string.Empty;

    [Required(ErrorMessage = "Review text is required.")]
    [StringLength(1000, MinimumLength = 10, ErrorMessage = "Review text should be between 10 and 1000 characters.")]
    public string ReviewText { get; set; } = string.Empty;

    [Range(1, 5, ErrorMessage = "Rating must be between 1 and 5.")]
    public int Rating { get; set; }

    [ForeignKey(nameof(Book))]
    public int BookId { get; set; }
    public virtual Book? Book { get; set; }

    [ForeignKey(nameof(Reviewer))]
    public int ReviewerId { get; set; }
    public virtual Reviewer? Reviewer { get; set; }

    public Review()
    {
        Book = null;
        Reviewer = null;
    }
}
