using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TheIndianSuperMarket.Models
{
    public class MailModel
    {
        public string CustomerName { get; set; }
        public string CustomerEmail { get; set; }
        public string Subject { get; set; }
        public string Message { get; set; }
    }
}
