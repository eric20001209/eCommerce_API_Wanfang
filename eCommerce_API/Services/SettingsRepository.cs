using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using eCommerce_API.Data;
using eCommerce_API.Dto;

namespace eCommerce_API.Services
{
	public class SettingsRepository : ISettings
	{
		private readonly FreightContext _context;
		private readonly rst374_cloud12Context _contextE;
		public SettingsRepository(FreightContext context, rst374_cloud12Context contextE)
		{
			_context = context;
			_contextE = contextE;
		}

		public int getOnlineShopId()
		{
			var onlineshop = _context.Settings.Where(s => s.Name == "online_shop_id").FirstOrDefault();
			if (onlineshop == null)
				return 1;
			var final = Convert.ToInt32(onlineshop.Value);
			return final;
		}
		public IEnumerable<FreightDto> getFreightSetting()
		{
			return _context.FreightSettings.Where(f => f.Active == true)
					.Select(i => new FreightDto
					{
						Id = i.Id.ToString(),
						Region = i.Region,
						Freight = i.Freight,
						FreeshippingActiveAmount = i.FreeshippingActiveAmount 
					}).OrderBy(i=>i.Region);
					
		}
	}
}
