using System;
using System.Collections.Generic;

namespace TheIndianSuperMarket.Models
{
    public partial class Suppliers
    {
        public Suppliers()
        {
            Products = new HashSet<Products>();
            ProvidedBy = new HashSet<ProvidedBy>();
            ProvidesDelivery = new HashSet<ProvidesDelivery>();
        }

        public int SupplierId { get; set; }
        public string SupplierName { get; set; }
        public string SupplierAddress { get; set; }
        public string SupplierPhone { get; set; }
        public string Contact { get; set; }
        public string Email { get; set; }

        public virtual ICollection<Products> Products { get; set; }
        public virtual ICollection<ProvidedBy> ProvidedBy { get; set; }
        public virtual ICollection<ProvidesDelivery> ProvidesDelivery { get; set; }
    }
}
