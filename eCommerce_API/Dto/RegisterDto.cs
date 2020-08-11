using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace eCommerce_API.Dto
{
    public class RegisterDto
    {
        [Required]
        [MaxLength(50, ErrorMessage = "The lengh is no more than 50")]
        public string name { get; set; }

        [Required]
        [EmailAddress]
        public string email { get; set; }
        public string password { get; set; }

        public int type { get; set; }

        public int accesslevel { get; set; }
    }
}
