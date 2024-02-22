using Microsoft.EntityFrameworkCore;

namespace Backend.Entities.Models;

[Keyless]
public class BookAuthor
{
	public int BookId { get; set; }
	public Book Book { get; set; }
	public int AuthorId { get; set; }
	public Author Author { get; set; }
}
