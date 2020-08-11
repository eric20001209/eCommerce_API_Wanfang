using System;
using System.Collections.Generic;

namespace eCommerce_API.Models
{
    public partial class Invoice
    {
        public int Id { get; set; }
        public int InvoiceNumber { get; set; }
        public int? Branch { get; set; }
        public byte? Type { get; set; }
        public byte SalesType { get; set; }
        public int CardId { get; set; }
        public bool SpecialShipto { get; set; }
        public string Shipto { get; set; }
        public decimal? Price { get; set; }
        public decimal? Tax { get; set; }
        public decimal? Freight { get; set; }
        public decimal? Total { get; set; }
        public DateTime? CommitDate { get; set; }
        public byte? PaymentType { get; set; }
        public bool Paid { get; set; }
        public bool Refunded { get; set; }
        public decimal? AmountPaid { get; set; }
        public string TransFailedReason { get; set; }
        public bool System { get; set; }
        public string Sales { get; set; }
        public string DebugInfo { get; set; }
        public bool NoIndividualPrice { get; set; }
        public bool GstInclusive { get; set; }
        public int Status { get; set; }
        public string CustPonumber { get; set; }
        public string SalesNote { get; set; }
        public byte ShippingMethod { get; set; }
        public string PickUpTime { get; set; }
        public string Name { get; set; }
        public string Company { get; set; }
        public string TradingName { get; set; }
        public string Email { get; set; }
        public string Address1 { get; set; }
        public string Address2 { get; set; }
        public string Address3 { get; set; }
        public string Postal1 { get; set; }
        public string Postal2 { get; set; }
        public string Postal3 { get; set; }
        public string Phone { get; set; }
        public string Fax { get; set; }
        public int Agent { get; set; }
        public DateTime RecordDate { get; set; }
        public double CustomerGst { get; set; }
        public bool Uploaded { get; set; }
        public int? StationId { get; set; }
        public string DeliveryNumber { get; set; }
        public string Barcode { get; set; }
        public bool? UploadedActivata { get; set; }
    }
}
