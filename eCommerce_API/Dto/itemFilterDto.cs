using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace eCommerce_API.Dto
{
    public class itemFilterDto
    {
        public int? branch { get; set; } = 1;
        public string Supplier { get; set; }
        public string Brand { get; set; }
        public string Cat { get; set; }
        public string SCat { get; set; }
        public string SsCat { get; set; }
        public bool? Hot { get; set; } = null;
        public bool? Skip { get; set; } = null;
        public bool? Clearance { get; set; } = null;
        public bool? ComingSoon { get; set; } = null;
        public bool? NewItem { get; set; } = null;
        public bool? Inactive { get; set; } = null;
        public bool? FreeDelivery { get; set; }
        public string KeyWord { get; set; }

    }
}
