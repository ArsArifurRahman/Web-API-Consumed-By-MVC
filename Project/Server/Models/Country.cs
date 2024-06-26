﻿using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Server.Models;

[Index(nameof(Name), IsUnique = true)]
public class Country
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }

    [Required(ErrorMessage = "Name is required.")]
    [StringLength(32, ErrorMessage = "Name cannot be longer than 32 characters.")]
    public string Name { get; set; } = string.Empty;

    public virtual ICollection<Author> Authors { get; set; }

    public Country()
    {
        Authors = new List<Author>();
    }
}
