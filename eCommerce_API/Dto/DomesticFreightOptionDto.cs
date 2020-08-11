using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace eCommerce_API.Dto
{
	public class DomesticFreightOptionDto
	{
		public int Id{ get; set; }
		public string Description { get; set; }
		public decimal Price { get; set; }
	}
}
