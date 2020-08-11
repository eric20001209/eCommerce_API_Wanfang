using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace eCommerce_API.Dto
{
    public class UserDto
    {
        public int id { get; set; }
        public int type { get; set; }
        [Required]
        public string name { get; set; }
        [EmailAddress]
        public string login_email { get; set; }
        public string password { get; set; }
        [Phone]
        public string phone { get; set; }
        public string address1 { get; set; }
        public string address2 { get; set; }
        public string address3 { get; set; }
        public string city { get; set; }
        public string country { get; set; }

        public IEnumerable<ShippingToDto> shiptoList;
    }
}
