using System;
using System.Collections.Generic;

namespace TheIndianSuperMarket.Models
{
    public partial class Purchases
    {
        public int TransactionId { get; set; }
        public int ProductId { get; set; }
        public int? Quantity { get; set; }
        public int? CustomerId { get; set; }
        public string PurchaseDate { get; set; }
        public double? Total { get; set; }

        public virtual Customers Customer { get; set; }
        public virtual Products Product { get; set; }

    }
}
