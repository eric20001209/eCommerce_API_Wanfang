using System;
using System.Collections.Generic;

namespace eCommerce_API.Models
{
    public partial class Trans
    {
        public int Id { get; set; }
        public int? Source { get; set; }
        public int? Dest { get; set; }
        public decimal Amount { get; set; }
        public decimal? DestAmount { get; set; }
        public bool Banked { get; set; }
        public int? TransBankId { get; set; }
        public int? TransDate { get; set; }
        public int Branch { get; set; }
        public bool Reconcile { get; set; }
        public bool IsDelete { get; set; }
        public int? StationId { get; set; }
        public bool IsLb { get; set; }
    }
}
