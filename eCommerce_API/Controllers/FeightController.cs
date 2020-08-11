using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using eCommerce_API.Data;
using eCommerce_API.Dto;
using eCommerce_API.Services;

namespace eCommerce_API.Controllers
{
    [Route("api/freight")]
    [ApiController]
    public class FeightController : ControllerBase
    {
		FreightContext _context = new FreightContext();
		private readonly ISettings _isettings;
		public FeightController(ISettings isettings)
		{
			_isettings = isettings;
		}
		public IActionResult getFreightUnitPrice()
		{
			var freight = _context.Settings.Where(s => s.Name == "freight_unit_price").FirstOrDefault();
			var freightUnitPrice = freight.Value;
			if (freightUnitPrice == null || freightUnitPrice == "")
				freightUnitPrice = "5";
			return Ok(freightUnitPrice);
		}

		[HttpGet("settings")]
		public IActionResult getFreightSettings()
		{
			/********   Default Oversea Freight   ****************/
			var freight = _context.Settings.Where(s => s.Name == "freight_unit_price").FirstOrDefault().Value;
			var freightUnitPrice = decimal.Parse(freight ?? "5");
			//if (freight == null || freight == "")
			//	freightUnitPrice = 5;

			/*********   Free shipping & Domestic Freight Settings   ********************/
			var freeshippingEnabled = _context.Settings.Where(s => s.Cat == "Freight Options" && s.Name == "free_shipping_enabled").FirstOrDefault().Value;
			var freeshippingInternational = _context.Settings.Where(s => s.Cat == "Freight Options" && s.Name == "free_shipping_international").FirstOrDefault().Value;
			var freeshippingDomestic = _context.Settings.Where(s => s.Cat == "Freight Options" && s.Name == "free_shipping_domestic").FirstOrDefault().Value ;
			var freeShippingActiveAmount = decimal.Parse( _context.Settings.Where(s => s.Name == "free_shipping_active_amount").FirstOrDefault().Value ?? "100");
			var domesticFreightOption = _context.Settings.Where(s => s.Cat == "Freight Options" && s.Name == "number_of_freight_options").FirstOrDefault().Value;
			int i_domesticFreightOption = int.Parse(domesticFreightOption ?? "0");
			List<DomesticFreightOptionDto> domesticFrieghtList = new List<DomesticFreightOptionDto>();
			for (int i = 1; i <= i_domesticFreightOption; i++)
			{ 
				var name = _context.Settings.Where(s => s.Cat == "Freight Options" && s.Name == "freight_option_name"+i.ToString()).FirstOrDefault().Value;
				var price = _context.Settings.Where(s => s.Cat == "Freight Options" && s.Name == "freight_option_price" + i.ToString()).FirstOrDefault().Value;
				decimal dprice = decimal.Parse(price??"0");
				domesticFrieghtList.Add(new DomesticFreightOptionDto { Id = i, Description = name, Price = dprice }) ;
			}

			return Ok( new { OverseaFreight = freightUnitPrice ,
							DomesticFreight = domesticFrieghtList,
							Freeshipping = freeshippingEnabled,
							FreeshppingActiveAmount = freeShippingActiveAmount
			});
		}

		[HttpGet("fixed")]
		public IActionResult getFixedFreightSettings()
		{
			var freeshippingEnabled = _context.Settings.Where(s => s.Cat == "Freight Options" && s.Name == "free_shipping_enabled").FirstOrDefault().Value;
			var freeshippingSetting = _isettings.getFreightSetting();
			return Ok(
				new{
					freeshippingEnabled,
					freeshippingSetting
				}
			);
		}
	}
}