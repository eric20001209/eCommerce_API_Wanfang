using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace eCommerce_API.Dto
{
    public class CartDto
    {
        public int card_id { get; set; }
        public double total_weight{ get; set; }
        public double? freight { get; set; }
        public double? tax { get; set; }
        public double customer_gst { get; set; } = 0.15;
        public double? sub_total { get; set; }
        public double? total { get; set; }
        public int total_points { get; set; }
        public bool oversea{ get; set; }

        public List<CartItemDto> cartItems = new List<CartItemDto>();
        public ShippingInfoDto shippingInfo = new ShippingInfoDto();
    }
}
