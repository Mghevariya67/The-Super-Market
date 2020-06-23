using System;
using System.Collections.Generic;

namespace TheIndianSuperMarket.Models
{
    public partial class ProvidesDelivery
    {
        public ProvidesDelivery()
        {
            ReceivedFrom = new HashSet<ReceivedFrom>();
        }

        public int DeliveryId { get; set; }
        public int SupplierId { get; set; }

        public virtual Suppliers Supplier { get; set; }
        public virtual ICollection<ReceivedFrom> ReceivedFrom { get; set; }
    }
}
