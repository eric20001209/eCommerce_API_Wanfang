using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace eCommerce_API.Dto
{
    public class ShippingInfoDto
    {
        public int id { get; set; }
        //public int type { get; set; }
        public int orderId { get; set; }
        [Required]
        public string sender { get; set; }
        [Phone]
        [Required]
        public string sender_phone { get; set; }
        public string sender_address { get; set; }
        public string sender_city { get; set; }
        public string sender_country { get; set; }
        [Required]
        public string receiver { get; set; }
        public string receiver_company { get; set; }
        public string receiver_address1 { get; set; }
        public string receiver_address2 { get; set; }
        public string receiver_address3 { get; set; }
        public string receiver_city { get; set; }
        public string receiver_country { get; set; }
        [Phone]
        [Required]
        public string receiver_phone { get; set; }
        public string receiver_contact { get; set; }
        public string note { get; set; }

        public bool oversea{ get; set; }

    }
}
