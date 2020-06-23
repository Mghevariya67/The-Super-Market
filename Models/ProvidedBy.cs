using System;
using System.Collections.Generic;

namespace TheIndianSuperMarket.Models
{
    public partial class ProvidedBy
    {
        public int ProductId { get; set; }
        public int SupplierId { get; set; }

        public virtual Products Product { get; set; }
        public virtual Suppliers Supplier { get; set; }
    }
}
