using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace eCommerce_API.Dto
{
    public class AddUserDto
    {
        [Required]
        [MaxLength(50,ErrorMessage = "The length is no more than 50")]
        public string name { get; set; }
        public string short_name { get; set; }
        public string trading_name { get; set; }

        [Required]
        [DataType(DataType.EmailAddress)]
        [EmailAddress]
        public string email { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string password { get; set; }
        public string phone { get; set; }
        public string address { get; set; }
        public string suburb { get; set; }
        public string City { get; set; }
        public string Country { get; set; }
    }
}
