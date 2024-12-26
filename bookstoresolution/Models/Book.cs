using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace bookstoresolution.Models;

public partial class Book
{
    public int BookId { get; set; }

    [Required]
   // [RegularExpression("^[a-zA-Z0-9]*$", ErrorMessage = "Only alphanumeric characters are allowed.")]
    public string Title { get; set; } = null!;

    [Required]
    [Range(1, 1000000)]
    public decimal Price { get; set; }

    [Required]
    [Range(0, 1000000)]
    public int Stock { get; set; }

    public int AuthorId { get; set; }

    public int CategoryId { get; set; }

    public virtual Author Author { get; set; } = null!;

    public virtual ICollection<BillDetail> BillDetails { get; set; } = new List<BillDetail>();

    public virtual Category Category { get; set; } = null!;
}
