using System;
using System.Collections.Generic;

namespace bookstoresolution.Models;

public partial class BillDetail
{
    public int BillDetailId { get; set; }

    public int BillId { get; set; }

    public int BookId { get; set; }

    public int Quantity { get; set; }

    public decimal TotalPrice { get; set; }

    public virtual Billing Bill { get; set; } = null!;

    public virtual Book Book { get; set; } = null!;


}
