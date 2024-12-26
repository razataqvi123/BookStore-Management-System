using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace bookstoresolution.Models;

public partial class Customer
{
    public int CustomerId { get; set; }

    [Required]
    [RegularExpression(@"^[^\d]*$", ErrorMessage = "Only characters are allowed.")]
    public string Name { get; set; } = null!;

    [Required]
    [RegularExpression(@"^[^@\s]+@[^@\s]+\.[^@\s]+$", ErrorMessage = "Enter a valid email address.")]
    public string Email { get; set; } = null!;

    [Required]
    [RegularExpression(@"^\+?[0-9\s\-()]{10,13}$", ErrorMessage = "Enter a valid phone number.")]

    public string? Phone { get; set; }

    [Required]
    [RegularExpression(@"^[a-zA-Z0-9\s,.\-/#]+$", ErrorMessage = "Enter a valid address.")]
    public string? Address { get; set; }

    public virtual ICollection<Billing> Billings { get; set; } = new List<Billing>();
}
