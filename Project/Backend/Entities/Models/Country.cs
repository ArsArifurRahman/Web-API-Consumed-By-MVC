using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Backend.Entities.Models;

public class Country
{
	[Key]
	[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
	public int Id { get; set; }

	[Required]
	[MaxLength(50, ErrorMessage = "Country must be up to 50 characters in length")]
	public string Name { get; set; }
	public virtual ICollection<Author> Authors { get; set; }
}
