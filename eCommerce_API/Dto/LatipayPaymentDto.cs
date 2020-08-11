using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace eCommerce_API.Dto
{
    public class LatipayPaymentDto
    {

        public string merchant_reference { get; set; }
        public string order_id { get; set; }
        public string currency { get; set; }
        public string amount { get; set; }
        public string payment_method{get;set;}
        public string status { get; set; }
        public string pay_time { get; set; }
        public string signature { get; set; }

        //public int id { get; set; }
        //public int invoice_number { get; set; }
        //public int payment_method { get; set; }
        //public double amount { get; set; }
    }
}
