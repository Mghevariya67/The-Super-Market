using System;
using System.Collections.Generic;

namespace TheIndianSuperMarket.Models
{
    public partial class ReceivedFrom
    {
        public int ProductId { get; set; }
        public int DeliveryId { get; set; }

        public virtual ProvidesDelivery Delivery { get; set; }
        public virtual Products Product { get; set; }
    }
}
