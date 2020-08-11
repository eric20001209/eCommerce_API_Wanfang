using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace eCommerce_API.Dto
{
    public class OrderDto
    {
        public int id { get; set; }
        public int branch { get; set; }
        public string branch_name { get; set; }
        public int card_id { get; set; }
        public string po_number { get; set; }
        public string status { get; set; }
        public int? invoice_number { get; set; }
        public decimal? TotalAmount_GstIncl { get; set; }
        public decimal? TotalAmount_GSTExcl { get; set; }
        public decimal? GstAmount { get; set; }
        public DateTime record_date { get; set; }
        public string shipto { get; set; }
        public bool special_shipto { get; set; } = false;
        public DateTime? date_shipped { get; set; }
        public decimal freight { get; set; }
        public string ticket { get; set; }
        public int sales { get; set; }
        public string sales_note { get; set; }
        public int shipping_method { get; set; }
        public DateTime pickup_time { get; set; }
        public int payment_type { get; set; }
        public bool paid { get; set; } = false;
        public double customer_gst { get; set; } = 0.15;
        public bool order_deleted { get; set; } = false;
        public string receiver_name { get; set; }
        public string receiver_phone { get; set; }
        
        public bool is_web_order { get; set; }
        public int web_order_status { get; set; }

    }
}
