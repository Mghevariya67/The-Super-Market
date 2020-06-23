using System;
using System.Collections.Generic;

namespace TheIndianSuperMarket.Models
{
    public partial class Customers
    {
        public Customers()
        {
            Payment = new HashSet<Payment>();
        }

        public int CustomerId { get; set; }
        public string CustomerFirstName { get; set; }
        public string CustomerLastName { get; set; }
        public string Address { get; set; }
        public string City { get; set; }
        public string PostalCode { get; set; }
        public string CustomerPhone { get; set; }
        public string CustomerEmail { get; set; }
        public string Password { get; set; }

        public virtual ICollection<Payment> Payment { get; set; }
    }
}
