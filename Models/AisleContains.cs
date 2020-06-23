using System;
using System.Collections.Generic;

namespace TheIndianSuperMarket.Models
{
    public partial class AisleContains
    {
        public int AisleNumber { get; set; }
        public int ProductId { get; set; }

        public virtual Aisles AisleNumberNavigation { get; set; }
        public virtual Products Product { get; set; }
    }
}
