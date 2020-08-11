using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace eCommerce_API.Models
{
    public class ShippingInfo
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int id { get; set; }
        [ForeignKey("orderId")]
        public Orders order { get; set; }
        public int orderId { get; set; }
        public string sender { get; set; }
        [Phone]
        public string sender_phone { get; set; }
        public string sender_address { get; set; }
        public string sender_city { get; set; }
        public string sender_country { get; set; }

        public string receiver { get; set; }
        public string receiver_company { get; set; }
        public string receiver_address1 { get; set; }
        public string receiver_address2 { get; set; }
        public string receiver_address3 { get; set; }
        public string receiver_city { get; set; }
        public string receiver_country { get; set; }
        [Phone]
        public string receiver_phone { get; set; }
        public string zip { get; set; }
        public string receiver_contact { get; set; }
        public string note { get; set; }
    }
}

