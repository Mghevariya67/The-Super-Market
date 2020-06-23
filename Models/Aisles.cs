using System;
using System.Collections.Generic;

namespace TheIndianSuperMarket.Models
{
    public partial class Aisles
    {
        public Aisles()
        {
            AisleContains = new HashSet<AisleContains>();
        }

        public int AisleNumber { get; set; }
        public string AisleName { get; set; }

        public virtual ICollection<AisleContains> AisleContains { get; set; }
    }
}
