using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace bookstoresolution.Models;

public partial class Category
{
    public int CategoryId { get; set; }

    [Required]
    [RegularExpression(@"^[^\d]*$", ErrorMessage = "Only characters are allowed.")]
    public string Name { get; set; } = null!;

    public virtual ICollection<Book> Books { get; set; } = new List<Book>();
}
