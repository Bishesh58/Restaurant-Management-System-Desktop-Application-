﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RestoManagement
{
    public class OrderHeader
    {
        public int orderId { get; set; }
        public DateTime generatedTimeDate { get; set; }
        public string percentage { get; set; }
        public decimal amount { get; set; }
        public string payType { get; set; }
    }
}
