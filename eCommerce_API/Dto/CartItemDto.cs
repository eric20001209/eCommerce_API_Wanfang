using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace eCommerce_API.Dto
{
    public class CartItemDto
    {
        public int id { get; set; }
        public int card_id { get; set; }
        public string code { get; set; }
        public string name { get; set; }
        public string quantity { get; set; } 
        public string sales_price { get; set; }
        public string supplier_code { get; set; }
        public string barcode { get; set; }
        public string points { get; set; }
        public double? weight { get; set; }
        public bool free_delevery { get; set; }
        public double total { get; set; }
        public int total_points { get; set; }
        public byte[] RowVersion { get; set; }
    }
}
