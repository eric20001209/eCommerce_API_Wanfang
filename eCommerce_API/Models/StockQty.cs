using System;
using System.Collections.Generic;

namespace eCommerce_API.Models
{
    public partial class StockQty
    {
        public int Id { get; set; }
        public int Code { get; set; }
        public double? Qty { get; set; }
        public int BranchId { get; set; }
        public decimal SupplierPrice { get; set; }
        public double? AllocatedStock { get; set; }
        public decimal AverageCost { get; set; }
        public decimal QposPrice { get; set; }
        public decimal? SpecialPrice { get; set; }
        public DateTime SpStartDate { get; set; }
        public DateTime SpEndDate { get; set; }
        public double LastStock { get; set; }
        public double? WarningStock { get; set; }
    }
}
