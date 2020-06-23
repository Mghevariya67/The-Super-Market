using System;
using System.Collections.Generic;

namespace TheIndianSuperMarket.Models
{
    public partial class Cart
    {
        public Cart()
        {
            Payment = new HashSet<Payment>();
        }

        public int Id { get; set; }
        public int CustomerId { get; set; }
        public int ProductId { get; set; }

        public virtual Customers Customer { get; set; }
        public virtual Products Product { get; set; }
        public virtual ICollection<Payment> Payment { get; set; }
    }
}
