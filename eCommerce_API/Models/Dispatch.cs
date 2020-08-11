using System;
using System.Collections.Generic;

namespace eCommerce_API.Models
{
    public partial class Dispatch
    {
        public int Kid { get; set; }
        public int Branch { get; set; }
        public int Id { get; set; }
        public int Code { get; set; }
        public string Name { get; set; }
        public double Qty { get; set; }
        public double QtyReceiving { get; set; }
        public double? QtyReceived { get; set; }
        public DateTime RecordDate { get; set; }
        public bool Received { get; set; }
        public DateTime? DateReceived { get; set; }
        public int? StaffReceived { get; set; }
    }
}
