using System;
using System.Collections.Generic;

namespace eCommerce_API.Models
{
    public partial class TranDetail
    {
        public int Kid { get; set; }
        public int Id { get; set; }
        public string InvoiceNumber { get; set; }
        public decimal? SourceBalance { get; set; }
        public decimal? DestBalance { get; set; }
        public DateTime? TransDate { get; set; }
        public int? StaffId { get; set; }
        public int? CardId { get; set; }
        public string Note { get; set; }
        public int? PaymentMethod { get; set; }
        public string PaymentRef { get; set; }
        public decimal Finance { get; set; }
        public decimal CurrencyLoss { get; set; }
        public decimal Credit { get; set; }
        public string PaidBy { get; set; }
        public string Bank { get; set; }
        public string Branch { get; set; }
        public int? CreditId { get; set; }
        public int? IsDeleted { get; set; }
        public int? StationId { get; set; }
    }
}
