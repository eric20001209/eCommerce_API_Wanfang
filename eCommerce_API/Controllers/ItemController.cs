using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using eCommerce_API.Data;
using eCommerce_API.Dto;
using eCommerce_API.Services;
using AutoMapper;


namespace eCommerce_API.Controllers
{
//    [Authorize]
    [Route("api/item")]
    [ApiController]
    public class ItemController : ControllerBase
    {
        private rst374_cloud12Context _context; // = new rst374_cloud12Context();
        private readonly ISettings _isettings;
        public ItemController(rst374_cloud12Context context, ISettings isettings)
        {
            _context = context;
            _isettings = isettings;
        }

        /****************item list*********************/
        [HttpGet()] 
        public IActionResult itemList([FromQuery] string supplier, [FromQuery] string brand, [FromQuery] string cat, [FromQuery] string scat, [FromQuery] string sscat
            , [FromQuery] bool? hot, [FromQuery] bool? skip, [FromQuery] bool? clearance, [FromQuery] bool? commingsoon, [FromQuery] bool? newitem, [FromQuery] bool inactive
            , [FromQuery] bool? freedelivery
            , [FromQuery] string keyword)
        {
            var myfilter = new itemFilterDto();
            myfilter.Supplier = supplier;
            myfilter.Brand = brand;
            myfilter.Cat = cat;
            myfilter.SCat = scat;
            myfilter.SsCat = sscat;
            myfilter.Hot = hot;
            myfilter.Skip = skip;
            myfilter.Clearance = clearance;
            myfilter.ComingSoon = commingsoon;
            myfilter.NewItem = newitem;
            myfilter.Inactive = inactive;
            myfilter.FreeDelivery = freedelivery;
            myfilter.KeyWord = keyword;

            var finalList = myList(myfilter);

            return Ok(finalList);
        }

        private List<ItemDto> myList([FromBody] itemFilterDto filter)
        {
            _context.ChangeTracker.QueryTrackingBehavior
                    = Microsoft.EntityFrameworkCore.QueryTrackingBehavior.NoTracking;
            
            var list = new List<ItemDto>();

            list = (from c in _context.CodeRelations
                    where 
                    c.IsWebsiteItem == true &&
                   (filter.Supplier != null ? c.Supplier == filter.Supplier : true)
                    && (filter.Brand != null ? c.Brand == filter.Brand : true)
                    && (filter.Cat != null ? c.Cat == filter.Cat : true)
                    && (filter.SCat != null ? c.SCat == filter.SCat : true)
                    && (filter.SsCat != null ? c.SsCat == filter.SsCat : true)
                    && (filter.Hot != null ? c.Hot == filter.Hot : true)
                    && (filter.Skip != null ? c.Skip == filter.Skip : true)
                    && (filter.Clearance != null ? c.Clearance == filter.Clearance : true)
                    && (filter.ComingSoon != null ? Convert.ToBoolean(c.ComingSoon) == filter.ComingSoon : true)
                    && (filter.NewItem != null ? c.NewItem == filter.NewItem : true)
                    && (filter.FreeDelivery != null ? c.FreeDelivery == filter.FreeDelivery : true)
                    && (filter.KeyWord != null ? c.Name.Contains(filter.KeyWord) || c.NameCn.Contains(filter.KeyWord)  || c.Code.ToString().Contains(filter.KeyWord)
                    || c.Brand.Contains(filter.KeyWord) || c.Cat.Contains(filter.KeyWord) || c.SCat.Contains(filter.KeyWord) || c.SsCat.Contains(filter.KeyWord)
                    : true)
                    join cb in _context.CodeBranch on c.Code equals cb.Code into g from cb in g.DefaultIfEmpty()
                    where cb.BranchId == _isettings.getOnlineShopId() && cb.Inactive == false
                    join pd in _context.ProductDetails on c.Code equals pd.Code into f from pd in f.DefaultIfEmpty()

                    select new ItemDto
                    {
                        Code = c.Code,
                        Supplier = c.Supplier,
                        SupplierCode = c.SupplierCode,
                        SupplierPrice = c.SupplierPrice,
                        ExpireDate = c.ExpireDate,
                        RefCode = c.RefCode,
                        AverageCost = c.AverageCost,
                        Name = c.Name,
                        NameCn = c.NameCn,
                        Brand = c.Brand,
                        Cat = c.Cat,
                        SCat = c.SCat,
                        SsCat = c.SsCat,
                        Hot = c.Hot,
                        Skip = c.Skip,
                        Clearance = c.Clearance,
                        ComingSoon = c.ComingSoon,
                        FreeDelivery = c.FreeDelivery,
                        Weight = c.Weight,
                        NewItem = c.NewItem,
                        Inactive = c.Inactive,
                        Popular = c.Popular,
                        IsSpecial = c.IsSpecial,
                        IsMemberOnly = c.IsMemberOnly,
                        IsWebsiteItem = c.IsWebsiteItem,
                        IsIdCheck = c.IsIdCheck,
                        NoDiscount = c.NoDiscount,
                        CommissionRate = c.CommissionRate,
                        SpecialCost = c.SpecialCost,
                        SpecialCostStartDate = c.SpecialCostStartDate,
                        SpecialCostEndDate = c.SpecialCostEndDate,
                        LevelRate1 = c.LevelRate1,
                        LevelRate2 = c.LevelRate2,
                        LevelRate3 = c.LevelRate3,
                        LevelRate4 = c.LevelRate4,
                        LevelRate5 = c.LevelRate5,
                        LevelRate6 = c.LevelRate6,
                        LevelRate7 = c.LevelRate7,
                        LevelRate8 = c.LevelRate8,
                        LevelRate9 = c.LevelRate9,
                        LevelPrice0 = c.LevelPrice0,
                        LevelPrice1 = c.LevelPrice1,
                        LevelPrice2 = c.LevelPrice2,
                        LevelPrice3 = c.LevelPrice3,
                        LevelPrice4 = c.LevelPrice4,
                        LevelPrice5 = c.LevelPrice5,
                        LevelPrice6 = c.LevelPrice6,
                        LevelPrice7 = c.LevelPrice7,
                        LevelPrice8 = c.LevelPrice8,
                        LevelPrice9 = c.LevelPrice9,
                        Price1 = cb.Price1 ?? 0,
                        Price2 = c.Price2,
                        Price3 = c.Price3,
                        TaxRate = c.TaxRate,
                        TaxCode = c.TaxCode,
                        CountryOfOrigin = c.CountryOfOrigin,
						Detail = pd.Details
                    })
                    .OrderBy(c=>c.Code).ToList();


			var myItemList = _context.CodeRelations.Where(c =>
					c.IsWebsiteItem == true &&
				   (filter.Supplier != null ? c.Supplier == filter.Supplier : true)
					&& (filter.Brand != null ? c.Brand == filter.Brand : true)
					&& (filter.Cat != null ? c.Cat == filter.Cat : true)
					&& (filter.SCat != null ? c.SCat == filter.SCat : true)
					&& (filter.SsCat != null ? c.SsCat == filter.SsCat : true)
					&& (filter.Hot != null ? c.Hot == filter.Hot : true)
					&& (filter.Skip != null ? c.Skip == filter.Skip : true)
					&& (filter.Clearance != null ? c.Clearance == filter.Clearance : true)
					&& (filter.ComingSoon != null ? Convert.ToBoolean(c.ComingSoon) == filter.ComingSoon : true)
					&& (filter.NewItem != null ? c.NewItem == filter.NewItem : true)
					&& (filter.FreeDelivery != null ? c.FreeDelivery == filter.FreeDelivery : true)
					&& (filter.KeyWord != null ? c.Name.Contains(filter.KeyWord) || c.NameCn.Contains(filter.KeyWord) || c.Code.ToString().Contains(filter.KeyWord)
					|| c.Brand.Contains(filter.KeyWord) || c.Cat.Contains(filter.KeyWord) || c.SCat.Contains(filter.KeyWord) || c.SsCat.Contains(filter.KeyWord)
					: true))
					.Join(_context.ProductDetails,
					c=>c.Code,
					pd => pd.Code,
					(c,pd)=> new ItemDto{
						Code = c.Code,
						Supplier = c.Supplier,
						SupplierCode = c.SupplierCode,
						SupplierPrice = c.SupplierPrice,
						ExpireDate = c.ExpireDate,
						RefCode = c.RefCode,
						AverageCost = c.AverageCost,
						Name = c.Name,
						NameCn = c.NameCn,
						Brand = c.Brand,
						Cat = c.Cat,
						SCat = c.SCat,
						SsCat = c.SsCat,
						Hot = c.Hot,
						Skip = c.Skip,
						Clearance = c.Clearance,
						ComingSoon = c.ComingSoon,
						FreeDelivery = c.FreeDelivery,
						Weight = c.Weight,
						NewItem = c.NewItem,
						Inactive = c.Inactive,
						Popular = c.Popular,
						IsSpecial = c.IsSpecial,
						IsMemberOnly = c.IsMemberOnly,
						IsWebsiteItem = c.IsWebsiteItem,
						IsIdCheck = c.IsIdCheck,
						NoDiscount = c.NoDiscount,
						CommissionRate = c.CommissionRate,
						SpecialCost = c.SpecialCost,
						SpecialCostStartDate = c.SpecialCostStartDate,
						SpecialCostEndDate = c.SpecialCostEndDate,
						LevelRate1 = c.LevelRate1,
						LevelRate2 = c.LevelRate2,
						LevelRate3 = c.LevelRate3,
						LevelRate4 = c.LevelRate4,
						LevelRate5 = c.LevelRate5,
						LevelRate6 = c.LevelRate6,
						LevelRate7 = c.LevelRate7,
						LevelRate8 = c.LevelRate8,
						LevelRate9 = c.LevelRate9,
						LevelPrice0 = c.LevelPrice0,
						LevelPrice1 = c.LevelPrice1,
						LevelPrice2 = c.LevelPrice2,
						LevelPrice3 = c.LevelPrice3,
						LevelPrice4 = c.LevelPrice4,
						LevelPrice5 = c.LevelPrice5,
						LevelPrice6 = c.LevelPrice6,
						LevelPrice7 = c.LevelPrice7,
						LevelPrice8 = c.LevelPrice8,
						LevelPrice9 = c.LevelPrice9,
						Price1 = c.Price1,
						Price2 = c.Price2,
						Price3 = c.Price3,
						TaxRate = c.TaxRate,
						TaxCode = c.TaxCode,
						CountryOfOrigin = c.CountryOfOrigin,
						Detail = pd.Details}
					);

			var result = list; // Mapper.Map<IEnumerable<ItemDto>>(myItemList).OrderBy(c => c.Code).ToList() ;



            return result;
        }
        List<BarcodeDto>barcodeList(int code)
        {
            var barcodes = new List<BarcodeDto>();
             
            barcodes = _context.Barcode.Where(b => b.ItemCode == code)
                .Select(b => new BarcodeDto
                {
                    ItemCode = code,
                    Barcode1 = b.Barcode1
                }).ToList();

            return barcodes;
        }


    }
}