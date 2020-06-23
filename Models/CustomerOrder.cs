using System;
using System.Collections.Generic;

namespace TheIndianSuperMarket.Models
{
    public partial class CustomerOrder
    {
        public int OrderId { get; set; }
        public string Name { get; set; }
        public string Date { get; set; }
        public string Total { get; set; }
    }
}
