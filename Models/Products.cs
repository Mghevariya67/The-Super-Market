using System;
using System.Collections.Generic;

namespace TheIndianSuperMarket.Models
{
    public partial class Products
    {
        public Products()
        {
            AisleContains = new HashSet<AisleContains>();
            ProvidedBy = new HashSet<ProvidedBy>();
            Purchases = new HashSet<Purchases>();
            ReceivedFrom = new HashSet<ReceivedFrom>();
        }

        public int ProductId { get; set; }
        public string ProductName { get; set; }
        public double PricePerCostUnit { get; set; }
        public string CostUnit { get; set; }
        public string DepartmentName { get; set; }
        public int? QuantityInStock { get; set; }
        public string Brand { get; set; }
        public string ProductionDate { get; set; }
        public string BestBeforeDate { get; set; }
        public int? Plu { get; set; }
        public long? Upc { get; set; }
        public int? Organic { get; set; }
        public string Cut { get; set; }
        public string Animal { get; set; }
        public string Description { get; set; }
        public int? SupplierId { get; set; }

        public virtual Departments DepartmentNameNavigation { get; set; }
        public virtual Suppliers Supplier { get; set; }
        public virtual ICollection<AisleContains> AisleContains { get; set; }
        public virtual ICollection<ProvidedBy> ProvidedBy { get; set; }
        public virtual ICollection<Purchases> Purchases { get; set; }
        public virtual ICollection<ReceivedFrom> ReceivedFrom { get; set; }
    }
}
