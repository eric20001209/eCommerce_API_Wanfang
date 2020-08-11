using System;
using System.Collections.Generic;

namespace eCommerce_API.Models
{
    public partial class Sales
    {
        public int Id { get; set; }
        public int InvoiceNumber { get; set; }
        public int Code { get; set; }
        public double Quantity { get; set; }
        public string Name { get; set; }
        public string Supplier { get; set; }
        public string SupplierCode { get; set; }
        public string SerialNumber { get; set; }
        public decimal CommitPrice { get; set; }
        public decimal SupplierPrice { get; set; }
        public byte? Status { get; set; }
        public byte? Shipby { get; set; }
        public string Ticket { get; set; }
        public string Note { get; set; }
        public DateTime? ShipDate { get; set; }
        public int? ProcessedBy { get; set; }
        public bool System { get; set; }
        public bool SysSpecial { get; set; }
        public int? Part { get; set; }
        public byte? PStatus { get; set; }
        public int? Owner { get; set; }
        public bool Used { get; set; }
        public int? StockAtSales { get; set; }
        public bool Kit { get; set; }
        public int? Krid { get; set; }
        public decimal NormalPrice { get; set; }
        public int? IncomeAccount { get; set; }
        public int? CostofsalesAccount { get; set; }
        public int? InventoryAccount { get; set; }
        public double DiscountPercent { get; set; }
        public string Cat { get; set; }
        public string SCat { get; set; }
        public string SsCat { get; set; }
        public decimal SalesTotal { get; set; }
        public int? StationId { get; set; }
        public string NameCn { get; set; }
        public string TaxCode { get; set; }
        public double? TaxRate { get; set; }
        public string Pack { get; set; }
        public int? PromoId { get; set; }
        public string PromoName { get; set; }
    }
}
