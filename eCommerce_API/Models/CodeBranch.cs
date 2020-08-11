using System;
using System.Collections.Generic;

namespace eCommerce_API.Models
{
    public partial class CodeBranch
    {
        public int Id { get; set; }
        public bool Inactive { get; set; }
        public int Code { get; set; }
        public int BranchId { get; set; }
        public decimal? Price1 { get; set; }
        public decimal? Price2 { get; set; }
        public int QposQtyBreak { get; set; }
        public bool Special { get; set; }
        public decimal? SpecialPrice { get; set; }
        public DateTime? SpecialPriceStartDate { get; set; }
        public DateTime? SpecialPriceEndDate { get; set; }
        public string StockLocation { get; set; }
        public double? ShelfQty { get; set; }
        public double? BranchLowStock { get; set; }
        public double ShelfQtyAdv { get; set; }
        public double BranchLowStockAdv { get; set; }
        public DateTime? SqaStartDate { get; set; }
        public DateTime? SqaEndDate { get; set; }
        public DateTime? LsaStartDate { get; set; }
        public DateTime? LsaEndDate { get; set; }
    }
}
