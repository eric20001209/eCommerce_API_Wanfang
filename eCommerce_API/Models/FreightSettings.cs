using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace eCommerce_API.Models
{
	public class FreightSettings
	{
		public int Id { get; set; }
		public bool Active { get; set; }
		public string Region{ get; set; }
		public decimal? Freight{ get; set; }
		public decimal? FreeshippingActiveAmount{ get; set; }
	}
}
