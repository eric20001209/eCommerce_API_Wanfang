using System;
using System.Collections.Generic;

namespace eCommerce_API.Models
{
    public partial class TranInvoice
    {
        public int Id { get; set; }
        public int TranId { get; set; }
        public int InvoiceNumber { get; set; }
        public decimal AmountApplied { get; set; }
        public bool? Purchase { get; set; }
        public int? IsDeleted { get; set; }
        public int? StationId { get; set; }
        public bool IsSettled { get; set; }
    }
}
