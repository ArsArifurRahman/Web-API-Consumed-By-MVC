﻿using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Server.Models;

[Index(nameof(FirstName), nameof(LastName), IsUnique = true)]
public class Reviewer
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

    public virtual ICollection<Critique> Critiques { get; set; }

    public Reviewer()
    {
        Critiques = new List<Critique>();
    }
}
