using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace eCommerce_API.Dto
{
    public class LatipayCreateInvoiceDto
    {
        [Required]
        public string user_id { get; set; }
        [Required]
        public string wallet_id { get; set; }
        [Required]
        public string amount { get; set; }
        public string product_name { get; set; }
        public int period_time { get; set; }
        public int app { get; set; }
        public string customer_order_id { get; set; }
        public string customer_reference { get; set; }
        public string return_url { get; set; }
        public string notify_url { get; set; }
        public bool qrcode { get; set; }
        public string signature { get; set; }
    }
}
