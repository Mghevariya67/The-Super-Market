using System;
using System.Collections.Generic;

namespace TheIndianSuperMarket.Models
{
    public partial class Departments
    {
        public Departments()
        {
            Employees = new HashSet<Employees>();
            Products = new HashSet<Products>();
        }

        public string DepartmentName { get; set; }

        public virtual ICollection<Employees> Employees { get; set; }
        public virtual ICollection<Products> Products { get; set; }
    }
}
