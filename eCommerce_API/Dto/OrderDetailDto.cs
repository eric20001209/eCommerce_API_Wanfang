using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace eCommerce_API.Dto
{
    public class OrderDetailDto
    {
        public int card_id { get; set; }
        public int invoice_number { get; set; }
        public int order_id { get; set; }
        public string status { get; set; }
        public string payment_method { get; set; }
        public double? freight { get; set; }
        public double? tax { get; set; }
        public double? sub_total { get; set; }
        public double? total { get; set; }
        public int total_points { get; set; }

        public List<OrderItemDto> orderItems = new List<OrderItemDto>();
        public ShippingInfoDto shippingInfo = new ShippingInfoDto();
        public List<FreightInfoDto> freightInfo = new List<FreightInfoDto>();
    }
}
