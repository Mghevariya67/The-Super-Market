using System;
using System.Collections.Generic;

namespace TheIndianSuperMarket.Models
{
    public partial class Employees
    {
        public int EmployeeId { get; set; }
        public string EmployeeName { get; set; }
        public string DepartmentName { get; set; }
        public string Position { get; set; }
        public long Sinumber { get; set; }
        public string EmployeeAddress { get; set; }
        public string EmployeePhone { get; set; }
        public int? Wage { get; set; }

        public virtual Departments DepartmentNameNavigation { get; set; }
    }
}
