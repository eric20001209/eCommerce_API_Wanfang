using System;
using System.Collections.Generic;

namespace eCommerce_API.Models
{
    public partial class PurchaseItem
    {
        public int Kid { get; set; }
        public int Id { get; set; }
        public int Code { get; set; }
        public string SupplierCode { get; set; }
        public string Name { get; set; }
        public double Qty { get; set; }
        public double QtyOrdered { get; set; }
        public decimal Price { get; set; }
        public bool SnEntered { get; set; }
        public bool Dispatched { get; set; }
    }
}
