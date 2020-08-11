using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using eCommerce_API.Dto;

namespace eCommerce_API.Services
{
	public interface ISettings
	{
		int getOnlineShopId();
		IEnumerable<FreightDto> getFreightSetting();
	}
}
