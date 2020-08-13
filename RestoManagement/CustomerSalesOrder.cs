using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RestoManagement
{
    public class CustomerSalesOrder
    {
        public string OrderNo { get; set; }
        public int Qty { get; set; }
        public string MenuOrderNo { get; set; }
        public decimal UnitPrice { get; set; }
        public decimal Subtotal { get; set; }
        public string DiscountDesc { get; set; }
        public decimal Discount { get; set; }
        public decimal TotalAmt { get; set; }
        public string PayType { get; set; }
        public bool Paid { get; set; }
        public bool Deleted { get; set; }
        public int UserId { get; set; }
        public DateTime CreationDatetime { get; set; }

    }
}
