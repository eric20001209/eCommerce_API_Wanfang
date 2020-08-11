using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace eCommerce_API.Dto
{
    public class LoginDto
    {
        public int id { get; set; }

        [MaxLength(50, ErrorMessage = "The length is no more than 50")]
        public string name { get; set; }

        [Required]
        [DataType(DataType.EmailAddress)]
        [EmailAddress]
        public string login_email { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string password { get; set; }
    }
}
