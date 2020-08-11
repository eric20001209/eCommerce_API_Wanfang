using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace eCommerce_API.Dto
{
    public class OrderFilterDto
    {
        public int id { get; set; } //user id
        public bool? inoviced { get; set; }
        public bool? paid { get; set; }
        public int? status { get; set; }
        public string customer { get; set; }
        public DateTime start { get; set; } = DateTime.MinValue;
        public DateTime end { get; set; } = DateTime.MaxValue;
        public string keyword { get; set; }
    }
}
