using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace eCommerce_API.Dto
{
    public class UpdateOrderShippingDto
    {
        public int order_id { get; set; }
        public byte? shipping_status { get; set; }
    }
}
