﻿using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Server.Models;

public class Country
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }

    [Required(ErrorMessage = "Name is required.")]
    [StringLength(32, ErrorMessage = "Name cannot be longer than 32 characters.")]
    public string? Name { get; set; }

    public virtual ICollection<Author>? Authors { get; set; }
}