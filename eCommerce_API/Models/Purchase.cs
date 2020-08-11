using System;
using System.Collections.Generic;

namespace eCommerce_API.Models
{
    public partial class Purchase
    {
        public int Id { get; set; }
        public int PoNumber { get; set; }
        public string InvNumber { get; set; }
        public int BranchId { get; set; }
        public int StaffId { get; set; }
        public int Type { get; set; }
        public int SupplierId { get; set; }
        public int BuyerId { get; set; }
        public string Shipto { get; set; }
        public int? Status { get; set; }
        public int? StatusOld { get; set; }
        public byte PaymentStatus { get; set; }
        public decimal Freight { get; set; }
        public decimal Total { get; set; }
        public decimal Tax { get; set; }
        public decimal TotalAmount { get; set; }
        public decimal AmountPaid { get; set; }
        public DateTime DateCreate { get; set; }
        public DateTime? DateReceived { get; set; }
        public DateTime? DateInvoiced { get; set; }
        public bool AlreadySent { get; set; }
        public string SentTo { get; set; }
        public string WhoSent { get; set; }
        public DateTime? TimeSent { get; set; }
        public string Note { get; set; }
        public bool? AllInStock { get; set; }
        public int? SalesOrderId { get; set; }
        public bool? SnEntered { get; set; }
        public byte Currency { get; set; }
        public double ExchangeRate { get; set; }
        public double GstRate { get; set; }
        public DateTime? TaxDate { get; set; }
        public int? SalesInv { get; set; }
        public bool CustGst { get; set; }
        public int? CustGstPid { get; set; }
        public DateTime? DeliveryDate { get; set; }
        public DateTime? ReceiveDate { get; set; }
        public DateTime? PaymentDate { get; set; }
        public int? ForPoNumber { get; set; }
        public string ForInvNumber { get; set; }
    }
}
