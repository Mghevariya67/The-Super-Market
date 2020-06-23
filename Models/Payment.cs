using System;
using System.Collections.Generic;

namespace TheIndianSuperMarket.Models
{
    public partial class Payment
    {
        public int PaymentId { get; set; }
        public int CartId { get; set; }
        public int CustomerId { get; set; }
        public string NameOnCard { get; set; }
        public string CardNumber { get; set; }
        public string Expiration { get; set; }
        public string Cvv { get; set; }

        public virtual Cart Cart { get; set; }
        public virtual Customers Customer { get; set; }
    }
}
