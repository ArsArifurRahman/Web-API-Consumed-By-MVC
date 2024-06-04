using System.ComponentModel.DataAnnotations.Schema;

namespace Server.Models;

public class BookAuthor
{
    [ForeignKey(nameof(Book))]
    public int BookId { get; set; }
    public Book? Book { get; set; }

    [ForeignKey(nameof(Author))]
    public int AuthorId { get; set; }
    public Author? Author { get; set; }
}
