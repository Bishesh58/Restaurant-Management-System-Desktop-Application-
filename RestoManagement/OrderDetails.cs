using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RestoManagement
{
    public class OrderDetails
    {
        public int orderId { get; set; }
        public int billId { get; set; }
        public DateTime generatedTimeDate { get; set; }
        public string percentage { get; set; }
        public decimal amount { get; set; }
        public string payType { get; set; }

        public int menuId { get; set; }
        public string menuName { get; set; }
        public int quantity { get; set; }
        public decimal price { get; set; }
        public decimal subTotal
        {
            get
            {
                return quantity * price;
            }

        }
    }
}
