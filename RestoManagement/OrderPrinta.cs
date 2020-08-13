using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RestoManagement
{
    public class OrderPrinta
    {

        public int OrderNo { get; set; }
        public string DiscountDesc { get; set; }
        public decimal Discount { get; set; }
        public decimal TotalAmt { get; set; }
        public decimal AmtDue { get; set; }
        public string PayType { get; set; }
        public int Paid { get; set; }
        public int Deleted { get; set; }
        public string CustomerName { get; set; }
        public string TableNo { get; set; }
        public int UserId { get; set; }
        public DateTime CreationDatetime { get; set; }
        
        public int Qty { get; set; }
        public string Description { get; set; }
        public decimal UnitPrice { get; set; }
        public decimal Subtotal { get; set; }

    }
}
