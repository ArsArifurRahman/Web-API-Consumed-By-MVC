using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Server.Models;

public class Review
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }

    [Required(ErrorMessage = "Headline is required.")]
    [StringLength(100, MinimumLength = 10, ErrorMessage = "Headline should be within 10 to 100 characters.")]
    public string? Headline { get; set; }

    [Required(ErrorMessage = "Review text is required.")]
    [StringLength(1000, MinimumLength = 10, ErrorMessage = "Review text should be within 10 to 1000 characters.")]
    public string? ReviewText { get; set; }

    public int Rating { get; set; }

    [ForeignKey(nameof(Book))]
    public int BookId { get; set; }
    public virtual Book? Book { get; set; }

    [ForeignKey(nameof(Reviewer))]
    public int ReviewerId { get; set; }
    public virtual Reviewer? Reviewer { get; set; }
}
