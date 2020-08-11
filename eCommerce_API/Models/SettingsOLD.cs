using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace eCommerce_API.Models
{
	public class SettingsOLD
	{
		public int Id { get; set; }
		public string Cat { get; set; }
		public string Name { get; set; }
		public string Value { get; set; }
		public string Description { get; set; }
		public bool Hidden { get; set; }
		public bool Bool_value { get; set; }
		public int Access { get; set; }
	}
}
