using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace eCommerce_API.Dto
{
    public class AddItemToCartDto
    {
        public int card_id { get; set; }
        public string code { get; set; }
        public string name { get; set; }
        public double quantity { get; set; } 
        public double sales_price { get; set; } = 0;
        public double points { get; set; } = 0;
        public string supplier_code { get; set; }
        public string barcode { get; set; }
    }
}
