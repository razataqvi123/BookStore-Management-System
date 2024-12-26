using System;
using System.Collections.Generic;

namespace bookstoresolution.Models;

public partial class Billing
{
    public int BillId { get; set; }

    public int CustomerId { get; set; }

    public DateTime? BillDate { get; set; }

    public decimal TotalAmount { get; set; }

    public ICollection<BillDetail> BillDetails { get; set; }

    public virtual Customer Customer { get; set; } = null!;
}
