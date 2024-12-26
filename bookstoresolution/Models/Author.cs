using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace bookstoresolution.Models;

public partial class Author
{
    public int AuthorId { get; set; }

    [Required]
    [RegularExpression(@"^[^\d]*$", ErrorMessage = "Only characters are allowed.")]
    public string Name { get; set; } = null!;

    [Required]
    [RegularExpression(@"^[^\d]*$", ErrorMessage = "Only characters are allowed.")]
    public string? Bio { get; set; }

    public virtual ICollection<Book> Books { get; set; } = new List<Book>();
}
