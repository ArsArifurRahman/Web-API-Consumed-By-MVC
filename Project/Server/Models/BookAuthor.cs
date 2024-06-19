using System.ComponentModel.DataAnnotations.Schema;

namespace Server.Models;

public class BookAuthor
{
    [ForeignKey(nameof(Book))]
    public int BookId { get; set; }
    public virtual Book? Book { get; set; }

    [ForeignKey(nameof(Author))]
    public int AuthorId { get; set; }
    public virtual Author? Author { get; set; }
}
