using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;


namespace eCommerce_API.Dto
{
    public class ItemDto
    {
        public int Code { get; set; }
        public string Supplier { get; set; }
        public string SupplierCode { get; set; }
        public decimal? SupplierPrice { get; set; }
        public DateTime? ExpireDate { get; set; }
        public string RefCode { get; set; }
        public decimal AverageCost { get; set; }
        public string Name { get; set; }
        public string NameCn { get; set; }
        public string Brand { get; set; }
        public string Cat { get; set; }
        public string SCat { get; set; }
        public string SsCat { get; set; }
        public bool? Hot { get; set; }
        public bool Skip { get; set; }
        public bool Clearance { get; set; }
        public byte? ComingSoon { get; set; }
        public bool NewItem { get; set; }
        public byte? Inactive { get; set; }
        public bool? Popular { get; set; }
        public bool IsSpecial { get; set; }
        public bool IsMemberOnly { get; set; }
        public bool IsWebsiteItem { get; set; }
        public bool IsIdCheck { get; set; }
        public bool NoDiscount { get; set; }
        public double CommissionRate { get; set; }
        public decimal? SpecialCost { get; set; }
        public DateTime? SpecialCostStartDate { get; set; }
        public DateTime? SpecialCostEndDate { get; set; }
        public double LevelRate1 { get; set; }
        public double LevelRate2 { get; set; }
        public double LevelRate3 { get; set; }
        public double LevelRate4 { get; set; }
        public double LevelRate5 { get; set; }
        public double LevelRate6 { get; set; }
        public double? LevelRate7 { get; set; }
        public double? LevelRate8 { get; set; }
        public double? LevelRate9 { get; set; }
        public decimal LevelPrice0 { get; set; }
        public decimal LevelPrice1 { get; set; }
        public decimal LevelPrice2 { get; set; }
        public decimal LevelPrice3 { get; set; }
        public decimal LevelPrice4 { get; set; }
        public decimal LevelPrice5 { get; set; }
        public decimal LevelPrice6 { get; set; }
        public decimal LevelPrice7 { get; set; }
        public decimal LevelPrice8 { get; set; }
        public decimal LevelPrice9 { get; set; }
        public decimal Price1 { get; set; }
        public decimal Price2 { get; set; }
        public decimal Price3 { get; set; }
        public double TaxRate { get; set; }
        public string TaxCode { get; set; }
        public string CountryOfOrigin { get; set; }
        public double? Qty { get; set; }
        public double? AllocatedStock { get; set; }
        public double? WarningStock { get; set; }
        public double? Weight { get; set; }
        public bool FreeDelivery { get; set; }
		public string Detail{ get; set; }

        public List<BarcodeDto> Barcodes = new List<BarcodeDto>();
    }
}
