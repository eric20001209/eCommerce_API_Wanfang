using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace eCommerce_API.Dto
{
    public class FreightInfoDto
    {
        public int invoice_number { get; set; } 
        public string ship_name { get; set; }
        public string ship_desc { get; set; }
        public string ticket { get; set; }
        public decimal price { get; set; }
        public int ship_id { get; set; }
    }
}
